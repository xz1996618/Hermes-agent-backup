using System.Diagnostics;
using System.Text.RegularExpressions;

using System.Text;

namespace GhProxyCli;

/// <summary>
/// Git HTTPS 代理配置管理。
/// 使用 git config --global 的 URL-specific 配置，仅对 GitHub 域名生效。
/// </summary>
public sealed class GitConfig
{
    private const string ProxyUrl = "http://127.0.0.1:16888";
    private static readonly string GitExe = LocateGit();

    /// <summary>是否已配置 GitHub 代理</summary>
    public bool IsConfigured
    {
        get
        {
            var (ok, output) = RunGit("config --global --get-all http.https://github.com.proxy");
            return ok && output.Contains(ProxyUrl);
        }
    }

    /// <summary>写入 GitHub 代理配置</summary>
    public bool Apply()
    {
        // 对每个 GitHub 域名单独配置代理，不影响其他域名
        var domains = new[]
        {
            "https://github.com",
            "https://api.github.com",
            "https://raw.githubusercontent.com",
            "https://gist.github.com",
            "https://assets-cdn.github.com",
            "https://github.global.ssl.fastly.net",
            "https://github.githubassets.com",
            "https://codeload.github.com",
            "https://objects.githubusercontent.com",
            "https://releases.githubusercontent.com",
        };

        var allOk = true;
        foreach (var url in domains)
        {
            // 先清除旧配置，再写新的
            RunGit($"config --global --unset-all http.{url}.proxy");
            var (ok, _) = RunGit($"config --global http.{url}.proxy {ProxyUrl}");
            if (!ok) allOk = false;
        }

        return allOk;
    }

    /// <summary>清除所有 GitHub 代理配置</summary>
    public bool Clear()
    {
        var domains = new[]
        {
            "https://github.com",
            "https://api.github.com",
            "https://raw.githubusercontent.com",
            "https://gist.github.com",
            "https://assets-cdn.github.com",
            "https://github.global.ssl.fastly.net",
            "https://github.githubassets.com",
            "https://codeload.github.com",
            "https://objects.githubusercontent.com",
            "https://releases.githubusercontent.com",
        };

        var allOk = true;
        foreach (var url in domains)
        {
            var (ok, _) = RunGit($"config --global --unset-all http.{url}.proxy");
            if (!ok) allOk = false;
        }

        return allOk;
    }

    /// <summary>备份当前配置（stop 时还原用）</summary>
    public bool Backup()
    {
        var (_, output) = RunGit("config --global --show-origin --get-regexp http\\..*\\.proxy");
        try
        {
            var backupDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ghproxy-cli");
            Directory.CreateDirectory(backupDir);
            File.WriteAllText(Path.Combine(backupDir, "git-config-backup.txt"), output);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static (bool Ok, string Output) RunGit(string args)
    {
        try
        {
            var psi = new ProcessStartInfo(GitExe, args)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
            };

            using var proc = Process.Start(psi)!;
            proc.WaitForExit(10_000);

            var output = proc.StandardOutput.ReadToEnd().Trim();
            var error = proc.StandardError.ReadToEnd().Trim();

            return (proc.ExitCode == 0, string.IsNullOrEmpty(output) ? error : output);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    private static string LocateGit()
    {
        // 常见 Git for Windows 安装路径
        var candidates = new[]
        {
            @"C:\Program Files\Git\cmd\git.exe",
            @"C:\Program Files\Git\bin\git.exe",
            @"C:\Program Files (x86)\Git\cmd\git.exe",
        };

        foreach (var path in candidates)
        {
            if (File.Exists(path)) return path;
        }

        // 兜底：从 PATH 找
        try
        {
            var psi = new ProcessStartInfo("where", "git.exe")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
            };
            using var proc = Process.Start(psi)!;
            proc.WaitForExit(3000);
            var output = proc.StandardOutput.ReadToEnd().Trim().Split('\n')[0].Trim();
            if (!string.IsNullOrEmpty(output) && File.Exists(output))
                return output;
        }
        catch { }

        return "git"; // 最后尝试 PATH
    }
}
