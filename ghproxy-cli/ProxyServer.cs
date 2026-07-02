using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GhProxyCli;

/// <summary>
/// 纯 TCP 正向代理核心。接受 HTTPS CONNECT 请求，将 TCP 隧道转发到优选 IP。
/// 不拦截/解密 TLS 流量，端到端加密。
/// </summary>
public sealed class ProxyServer : IDisposable
{
    private readonly int _port;
    private readonly IpSelector _ipSelector;
    private TcpListener? _listener;
    private CancellationTokenSource? _cts;
    private Task? _loopTask;
    private bool _running;

    public ProxyServer(int port, IpSelector ipSelector)
    {
        _port = port;
        _ipSelector = ipSelector;
    }

    /// <summary>是否正在运行</summary>
    public bool IsRunning => _running;

    /// <summary>启动代理服务器（不阻塞）</summary>
    public void Start()
    {
        if (_running) return;

        _cts = new CancellationTokenSource();
        _listener = new TcpListener(IPAddress.Loopback, _port);
        _listener.Start();
        _running = true;

        _loopTask = AcceptLoopAsync(_cts.Token);
    }

    /// <summary>停止代理服务器</summary>
    public async Task StopAsync()
    {
        if (!_running) return;

        _running = false;
        _cts?.Cancel();

        try
        {
            _listener?.Stop();
        }
        catch { /* 关闭监听器时的异常忽略 */ }

        if (_loopTask != null)
        {
            try { await _loopTask; } catch { /* 任务取消异常 */ }
        }

        _listener = null;
        _cts?.Dispose();
        _cts = null;
    }

    private async Task AcceptLoopAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                var client = await _listener!.AcceptTcpClientAsync(ct);
                // 不 await，并发处理多个连接
                _ = HandleClientAsync(client, ct);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (ObjectDisposedException)
            {
                break;
            }
            catch
            {
                // 单个 accept 失败不崩溃
            }
        }
    }

    private async Task HandleClientAsync(TcpClient client, CancellationToken ct)
    {
        try
        {
            using (client)
            {
                // 读超时防止挂死
                client.ReceiveTimeout = 30_000;
                client.SendTimeout = 30_000;

                var stream = client.GetStream();
                var buffer = new byte[8192];
                var totalRead = 0;

                // 读取 CONNECT 请求行（可能在多个 TCP 段中到达）
                string? requestLine = null;
                while (totalRead < buffer.Length)
                {
                    var bytesRead = await stream.ReadAsync(buffer.AsMemory(totalRead, buffer.Length - totalRead), ct);
                    if (bytesRead == 0) return; // 连接提前关闭

                    totalRead += bytesRead;

                    // 查找 CRLF 或 LF
                    var endIndex = -1;
                    for (int i = 0; i < totalRead - 1; i++)
                    {
                        if (buffer[i] == '\r' && buffer[i + 1] == '\n')
                        {
                            endIndex = i;
                            break;
                        }
                        if (buffer[i] == '\n')
                        {
                            endIndex = i;
                            break;
                        }
                    }

                    if (endIndex >= 0)
                    {
                        requestLine = Encoding.ASCII.GetString(buffer, 0, endIndex).Trim();
                        break;
                    }
                }

                if (string.IsNullOrEmpty(requestLine))
                    return;

                // 解析 "CONNECT host:port HTTP/1.1"
                if (!TryParseConnect(requestLine, out var host, out var port))
                    return;

                // 只代理 GitHub 域名
                if (!IsGitHubDomain(host))
                    return;

                // 获取优选 IP
                IPAddress targetIp;
                if (!_ipSelector.TryGetIp(host, out var cachedIp) || cachedIp == null)
                {
                    // 该域名没有缓存 IP 时，用 DNS 解析
                    var ips = await Dns.GetHostAddressesAsync(host, ct);
                    targetIp = ips.FirstOrDefault(i => i.AddressFamily == AddressFamily.InterNetwork)
                               ?? ips[0];
                }
                else
                {
                    targetIp = cachedIp;
                }

                // 连接到目标 IP（带超时，每次重试使用新 TcpClient）
                TcpClient? upstream = null;
                var connected = false;

                try
                {
                    // 先试优选 IP
                    upstream = NewTcpClient();
                    if (await TryConnectAsync(upstream, targetIp, port, ct))
                    {
                        connected = true;
                        goto connected;
                    }

                    // 优选 IP 不可达 → 尝试 ghproxy 已知的所有 GitHub IP（来自缓存）
                    var allKnownIps = LoadAllCachedGitHubIps();
                    foreach (var fallbackIp in allKnownIps)
                    {
                        if (fallbackIp.Equals(targetIp))
                            continue;

                        upstream.Dispose();
                        upstream = NewTcpClient();
                        if (await TryConnectAsync(upstream, fallbackIp, port, ct))
                        {
                            connected = true;
                            goto connected;
                        }
                    }

                    return; // 所有 IP 都不可达

                    connected:;
                }
                finally
                {
                    if (!connected)
                        upstream?.Dispose();
                }

                // 回复 200 Connection Established
                var responseBytes = "HTTP/1.1 200 Connection Established\r\n\r\n"u8.ToArray();
await stream.WriteAsync(responseBytes, ct);

                using (upstream!)
                {
                    // 双向对拷（两个方向独立转发）
                    var upstreamStream = upstream.GetStream();
                    var task1 = CopyToAsync(stream, upstreamStream, ct);
                    var task2 = CopyToAsync(upstreamStream, stream, ct);

                    await Task.WhenAny(task1, task2);
                    // 一端断开，另一端自然结束
                }
            }
        }
        catch (OperationCanceledException) { }
        catch (ObjectDisposedException) { }
        catch (IOException) { }
        catch { /* 单个连接异常不停止服务 */ }
    }

    private static async Task CopyToAsync(NetworkStream src, NetworkStream dst, CancellationToken ct)
    {
        var buffer = new byte[65536];
        try
        {
            while (!ct.IsCancellationRequested)
            {
                var bytesRead = await src.ReadAsync(buffer.AsMemory(0, buffer.Length), ct);
                if (bytesRead == 0) break;
                await dst.WriteAsync(buffer.AsMemory(0, bytesRead), ct);
                await dst.FlushAsync(ct);
            }
        }
        catch { /* 连接断开正常 */ }
    }

    /// <summary>带超时的 TCP 连接，超时 8 秒后尝试下一个 IP</summary>
    private static async Task<bool> TryConnectAsync(TcpClient client, IPAddress ip, int port, CancellationToken ct)
    {
        try
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            timeoutCts.CancelAfter(TimeSpan.FromSeconds(3));
            await client.ConnectAsync(ip, port, timeoutCts.Token);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>创建预设超时的 TcpClient</summary>
    private static TcpClient NewTcpClient()
    {
        return new TcpClient
        {
            ReceiveTimeout = 30_000,
            SendTimeout = 30_000,
        };
    }

    /// <summary>从 ghproxy IP 缓存中读取所有已知 GitHub 独立 IP，排除非 GitHub 基础设施的 CDN IP</summary>
    private static List<IPAddress> LoadAllCachedGitHubIps()
    {
        try
        {
            var stateDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ghproxy-cli");
            var cacheFile = Path.Combine(stateDir, "ip-cache.json");
            if (!File.Exists(cacheFile))
                return [];

            var json = File.ReadAllText(cacheFile);
            var cache = System.Text.Json.JsonSerializer.Deserialize<IpCacheEntry>(json);
            if (cache?.Entries == null || cache.Entries.Count == 0)
                return [];

            // 只保留 GitHub 自己的基础设施 IP (140.82.xxx.xxx)
            // 排除 CDN IP (13.107.xxx.xxx, 185.199.xxx.xxx) 因为那些域名敏感，TLS 会报证书错误
            var unique = new HashSet<IPAddress>();
            foreach (var ipStr in cache.Entries.Values)
            {
                if (IPAddress.TryParse(ipStr, out var ip))
                {
                    var bytes = ip.GetAddressBytes();
                    if (bytes.Length == 4 && bytes[0] == 140 && bytes[1] == 82)
                        unique.Add(ip);
                }
            }
            return unique.ToList();
        }
        catch
        {
            return [];
        }
    }

    private sealed class IpCacheEntry
    {
        public DateTime Timestamp { get; set; }
        public Dictionary<string, string> Entries { get; set; } = [];
    }

    private static bool TryParseConnect(string requestLine, out string host, out int port)
    {
        host = "";
        port = 0;

        var parts = requestLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2) return false;

        var authority = parts[1]; // "github.com:443" 或 "[::1]:443"
        var lastColon = authority.LastIndexOf(':');
        if (lastColon < 0) return false;

        host = authority[..lastColon];
        // 移除 IPv6 方括号
        if (host.StartsWith('[') && host.EndsWith(']'))
            host = host[1..^1];

        return int.TryParse(authority[(lastColon + 1)..], out port);
    }

    /// <summary>判断域名是否 GitHub 相关</summary>
    private static bool IsGitHubDomain(string host)
    {
        return IpSelector.GitHubDomains.Any(d =>
            host.Equals(d, StringComparison.OrdinalIgnoreCase) ||
            host.EndsWith("." + d, StringComparison.OrdinalIgnoreCase));
    }

    public void Dispose()
    {
        _cts?.Cancel();
        _listener?.Stop();
        _cts?.Dispose();
    }
}
