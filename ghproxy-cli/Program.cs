using System.Diagnostics;
using System.Text;
using GhProxyCli;

var stateDir = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "ghproxy-cli");
Directory.CreateDirectory(stateDir);

var pidFile = Path.Combine(stateDir, "daemon.pid");
var logFile = Path.Combine(stateDir, "daemon.log");

const int ProxyPort = 16888;

var cmd = args.Length > 0 ? args[0].ToLowerInvariant() : "status";

switch (cmd)
{
    case "start":
        await CmdStartAsync();
        break;

    case "stop":
        await CmdStopAsync();
        break;

    case "status":
        CmdStatus();
        break;

    case "refresh":
        await CmdRefreshAsync();
        break;

    case "--daemon":
        await RunDaemonAsync();
        break;

    default:
        Console.Error.WriteLine($"用法: ghproxy-cli start|stop|status|refresh");
        return 1;
}

return 0;

// ============================================================
// 命令实现
// ============================================================

async Task CmdStartAsync()
{
    // 1. 检查是否已在运行
    if (IsDaemonRunning(out var existingPid))
    {
        Console.WriteLine($"代理已在运行中 (PID {existingPid})");
        return;
    }

    Console.WriteLine("正在检测 GitHub 网络状态...");

    // 2. 加载 IP 缓存或重新测速
    var selector = new IpSelector();
    using (selector)
    {
        if (!selector.TryLoadCache())
        {
            Console.WriteLine("正在 DNS 解析+Ping 测速...");
            await selector.RefreshAsync(force: true);
        }

        // 3. 输出 IP 状态
        Console.WriteLine($"已监控 {selector.Domains.Count} 个 GitHub 域名");
        var domainsWithIps = selector.Domains
            .Select(d => (Domain: d, Ip: selector.TryGetIp(d, out var ip) ? ip?.ToString() : "未解析"))
            .ToList();

        foreach (var (domain, ip) in domainsWithIps.Take(5))
            Console.WriteLine($"  {domain} → {ip}");

        Console.WriteLine($"  ... 等 {selector.Domains.Count - 5} 个域名");

        // 4. 启动后台守护进程
        var daemonExe = GetDaemonPath();
        if (!File.Exists(daemonExe))
        {
            // 如果编译后 exe 不存在，用 dotnet run
            daemonExe = "dotnet";
        }

        var psi = new ProcessStartInfo
        {
            FileName = daemonExe,
            Arguments = daemonExe == "dotnet"
                ? $"run --project \"{GetProjectDir()}\" -- --daemon"
                : "--daemon",
            UseShellExecute = true, // 脱离父进程
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            WorkingDirectory = GetProjectDir(),
        };

        var daemon = Process.Start(psi);
        if (daemon == null)
        {
            Console.Error.WriteLine("错误: 无法启动代理守护进程");
            return;
        }

        // 5. 等待守护进程就绪
        File.WriteAllText(pidFile, daemon.Id.ToString());
        await WaitForReadyAsync();

        Console.WriteLine($"代理已启动 (PID {daemon.Id})...");

        // 6. 配置 Git
        Console.WriteLine("正在配置 Git 代理...");
        var gitConfig = new GitConfig();
        gitConfig.Backup();
        gitConfig.Apply();

        var sshConfig = new SshConfig();
        sshConfig.Apply();

        Console.WriteLine($"✅ 代理运行在 127.0.0.1:{ProxyPort}");
        Console.WriteLine("✅ Git HTTPS 代理已配置（仅 GitHub 域名）");
        Console.WriteLine("✅ SSH 代理已配置");
        Console.WriteLine();
        Console.WriteLine("使用 ghproxy-cli stop 停止代理并还原配置");
    }
}

Task CmdStopAsync()
{
    Console.WriteLine("正在停止代理...");

    // 1. 清除 Git 配置
    Console.WriteLine("正在还原 Git 代理配置...");
    var gitConfig = new GitConfig();
    gitConfig.Clear();

    var sshConfig = new SshConfig();
    sshConfig.Clear();

    // 2. 停止守护进程
    if (IsDaemonRunning(out var pid))
    {
        try
        {
            var proc = Process.GetProcessById(pid);
            proc.Kill(entireProcessTree: true);

            // 等进程退出
            proc.WaitForExit(5000);
            Console.WriteLine($"已停止代理进程 (PID {pid})");
        }
        catch (ArgumentException)
        {
            Console.WriteLine("代理进程已停止");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"停止进程失败: {ex.Message}");
        }
    }

    // 3. 清理文件
    try
    {
        if (File.Exists(pidFile)) File.Delete(pidFile);
        if (File.Exists(Path.Combine(stateDir, "ready.signal"))) File.Delete(Path.Combine(stateDir, "ready.signal"));
    }
    catch { }

    Console.WriteLine("✅ 代理已停止，Git 配置已还原");
    return Task.CompletedTask;
}

void CmdStatus()
{
    if (IsDaemonRunning(out var pid))
    {
        // 检查端口
        var portTest = TestPort(ProxyPort);
        Console.WriteLine($"✅ 代理运行中 (PID {pid}), 端口 {ProxyPort}: {(portTest ? "已监听" : "⚠️ 未监听")}");

        var gitOk = new GitConfig().IsConfigured;
        Console.WriteLine($"Git 代理: {(gitOk ? "已配置" : "未配置")}");

        var sshOk = new SshConfig().IsConfigured;
        Console.WriteLine($"SSH 代理: {(sshOk ? "已配置" : "未配置")}");

        // 显示最近日志
        if (File.Exists(logFile))
        {
            try
            {
                // 使用共享读模式避免与守护进程的写锁冲突
                using var fs = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var reader = new StreamReader(fs);
                var allLines = new List<string>();
                while (reader.ReadLine() is { } line)
                    allLines.Add(line);
                var tail = allLines.Count > 5 ? allLines[^5..] : allLines;
                Console.WriteLine("最近日志:");
                foreach (var t in tail)
                    Console.WriteLine($"  {t}");
            }
            catch
            {
                // 日志不可读时跳过
            }
        }
    }
    else
    {
        Console.WriteLine("❌ 代理未运行");
        Console.WriteLine($"使用 ghproxy-cli start 启动");
    }
}

async Task CmdRefreshAsync()
{
    Console.WriteLine("正在刷新 IP 列表（DNS解析 + Ping测速）...");
    using var selector = new IpSelector();
    await selector.RefreshAsync(force: true);

    Console.WriteLine("IP 列表已刷新:");
    foreach (var domain in selector.Domains)
    {
        if (selector.TryGetIp(domain, out var ip))
            Console.WriteLine($"  {domain} → {ip}");
    }
}

// ============================================================
// 守护进程模式（后台运行）
// ============================================================

async Task RunDaemonAsync()
{
    // 重定向日志（允许共享读，方便 status 命令读取）
    using var logWriter = new StreamWriter(
        new FileStream(logFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite),
        Encoding.UTF8);
    var origOut = Console.Out;
    Console.SetOut(logWriter);
    Console.Error.WriteLine($"守护进程启动于 {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

    using var selector = new IpSelector();

    // 加载缓存或刷新 IP
    if (!selector.TryLoadCache())
    {
        Console.Error.WriteLine("首次运行，正在测速...");
        await selector.RefreshAsync(force: true);
    }

    // 启动代理
    using var proxy = new ProxyServer(ProxyPort, selector);
    proxy.Start();
    Console.Error.WriteLine($"代理监听 127.0.0.1:{ProxyPort}");

    // 通知启动完成
    File.WriteAllText(Path.Combine(stateDir, "ready.signal"), "ready");

    // 定期刷新 IP（每 30 分钟重新测速一次）
    using var timer = new PeriodicTimer(TimeSpan.FromMinutes(30));
    _ = Task.Run(async () =>
    {
        while (await timer.WaitForNextTickAsync())
        {
            try
            {
                Console.Error.WriteLine("定期刷新 IP 列表...");
                await selector.RefreshAsync(force: true);
                Console.Error.WriteLine("IP 列表刷新完成");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"IP 刷新失败: {ex.Message}");
            }
        }
    });

    // 保持进程运行，直到收到退出信号
    // 监听停止标记文件
    var stopFile = Path.Combine(stateDir, "stop.signal");
    while (proxy.IsRunning)
    {
        if (File.Exists(stopFile))
        {
            Console.Error.WriteLine("检测到停止信号");
            File.Delete(stopFile);
            break;
        }
        await Task.Delay(1000);
    }

    Console.Error.WriteLine("正在停止代理...");
    await proxy.StopAsync();
    Console.Error.WriteLine("代理已停止");
}

// ============================================================
// 辅助方法
// ============================================================

bool IsDaemonRunning(out int pid)
{
    pid = 0;
    if (!File.Exists(pidFile)) return false;

    try
    {
        pid = int.Parse(File.ReadAllText(pidFile).Trim());
        var proc = Process.GetProcessById(pid);
        return !proc.HasExited;
    }
    catch
    {
        // PID 文件过期
        try { File.Delete(pidFile); } catch { }
        return false;
    }
}

async Task WaitForReadyAsync(int timeoutMs = 10_000)
{
    var readyFile = Path.Combine(stateDir, "ready.signal");
    var sw = Stopwatch.StartNew();

    while (sw.ElapsedMilliseconds < timeoutMs)
    {
        if (File.Exists(readyFile))
        {
            await Task.Delay(500); // 确保端口已绑定
            return;
        }
        await Task.Delay(200);
    }

    Console.Error.WriteLine("警告: 代理启动超时，但可能仍在后台启动中");
}

bool TestPort(int port)
{
    try
    {
        using var client = new System.Net.Sockets.TcpClient();
        client.Connect(System.Net.IPAddress.Loopback, port);
        return true;
    }
    catch
    {
        return false;
    }
}

string GetDaemonPath()
{
    var exePath = Environment.ProcessPath;
    if (!string.IsNullOrEmpty(exePath))
    {
        var dir = Path.GetDirectoryName(exePath)!;
        var name = Path.GetFileNameWithoutExtension(exePath);
        // 尝试已发布的可执行文件
        var published = Path.Combine(dir, $"{name}.exe");
        if (File.Exists(published))
            return published;
    }
    return "dotnet";
}

string GetProjectDir()
{
    // 从 exe 路径推断项目目录
    var exeDir = Path.GetDirectoryName(Environment.ProcessPath);
    if (exeDir != null && File.Exists(Path.Combine(exeDir, "ghproxy-cli.csproj")))
        return exeDir;

    // 默认项目路径
    return @"E:\13_WorkSpaceForHermes\ghproxy-cli";
}
