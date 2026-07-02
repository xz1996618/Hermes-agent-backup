using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace GhProxyCli;

/// <summary>
/// DNS 解析 + ICMP Ping 测速，选出 GitHub 各域名的最优 IP。
/// 纯本地，不调用任何外部服务。
/// </summary>
public sealed class IpSelector : IDisposable
{
    private readonly HttpClient _httpClient = new() { Timeout = TimeSpan.FromSeconds(5) };
    private readonly string _stateDir;
    private readonly string _cacheFile;
    private Dictionary<string, IPAddress> _domainIpMap = new(StringComparer.OrdinalIgnoreCase);
    private DateTime _lastRefresh = DateTime.MinValue;

    /// <summary>
    /// GitHub 关联域名（只代理这些域名）
    /// </summary>
    public static readonly string[] GitHubDomains =
    [
        "github.com",
        "api.github.com",
        "raw.githubusercontent.com",
        "gist.github.com",
        "assets-cdn.github.com",
        "github.global.ssl.fastly.net",
        "github.githubassets.com",
        "codeload.github.com",
        "objects.githubusercontent.com",
        "releases.githubusercontent.com",
        "pipelines.actions.githubusercontent.com",
        "github-releases.githubusercontent.com",
        "media.githubusercontent.com",
    ];

    public IpSelector()
    {
        _stateDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "ghproxy-cli");
        Directory.CreateDirectory(_stateDir);
        _cacheFile = Path.Combine(_stateDir, "ip-cache.json");
    }

    /// <summary>所有受监控的域名</summary>
    public IReadOnlyCollection<string> Domains => GitHubDomains;

    /// <summary>获取域名对应的优选 IP（若已缓存）</summary>
    public bool TryGetIp(string domain, out IPAddress? ip) =>
        _domainIpMap.TryGetValue(domain, out ip) && ip != null;

    /// <summary>刷新所有域名的优选 IP（后台线程，不阻塞）</summary>
    public async Task RefreshAsync(bool force = false)
    {
        if (!force && _lastRefresh > DateTime.Now.AddMinutes(-5))
            return;

        _lastRefresh = DateTime.Now;

        // 1. 解析所有域名 → 收集去重 IP 列表
        var uniqueIps = new Dictionary<IPAddress, string>(IPAddressComparer.Instance);
        foreach (var domain in GitHubDomains)
        {
            try
            {
                var ips = await Dns.GetHostAddressesAsync(domain);
                foreach (var ip in ips)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork) // IPv4 only
                        uniqueIps.TryAdd(ip, domain);
                }
            }
            catch
            {
                // 个别域名解析失败不阻塞整体
            }
        }

        if (uniqueIps.Count == 0)
        {
            // 兜底：用已知的 GitHub IP
            uniqueIps[IPAddress.Parse("140.82.114.3")] = "github.com";
            uniqueIps[IPAddress.Parse("140.82.112.3")] = "github.com";
        }

        // 2. Ping 测速（并发）
        var pingTasks = uniqueIps.Keys.Select(ip => PingAsync(ip));
        var results = await Task.WhenAll(pingTasks);

        var scored = new List<(IPAddress Ip, long LatencyMs, string SourceDomain)>();
        var idx = 0;
        foreach (var (ip, domain) in uniqueIps)
        {
            var latency = results[idx++];
            if (latency >= 0) // -1 = ping 失败
                scored.Add((ip, latency, domain));
        }

        if (scored.Count == 0)
        {
            // 所有 ping 都失败时，选第一个能解析到的
            foreach (var (ip, domain) in uniqueIps)
            {
                scored.Add((ip, 9999, domain));
            }
        }

        // 3. 按延迟排序
        scored.Sort((a, b) => a.LatencyMs.CompareTo(b.LatencyMs));

        // 4. 构建域名→最优IP映射
        var newMap = new Dictionary<string, IPAddress>(StringComparer.OrdinalIgnoreCase);
        foreach (var domain in GitHubDomains)
        {
            // 为该域名选一个同源的最优IP
            var best = scored.FirstOrDefault(s =>
                s.SourceDomain.Equals(domain, StringComparison.OrdinalIgnoreCase));
            if (best == default)
                best = scored.FirstOrDefault(); // 用全局最优
            if (best != default)
                newMap[domain] = best.Ip;
        }

        _domainIpMap = newMap;

        // 5. 缓存到文件
        try
        {
            var cache = new IpCache
            {
                Timestamp = DateTime.Now,
                Entries = newMap.ToDictionary(
                    kv => kv.Key,
                    kv => kv.Value.ToString()),
            };
            var json = System.Text.Json.JsonSerializer.Serialize(cache);
            await File.WriteAllTextAsync(_cacheFile, json);
        }
        catch { /* 缓存写入失败不影响运行 */ }
    }

    /// <summary>尝试从缓存恢复</summary>
    public bool TryLoadCache()
    {
        try
        {
            if (!File.Exists(_cacheFile)) return false;
            var json = File.ReadAllText(_cacheFile);
            var cache = System.Text.Json.JsonSerializer.Deserialize<IpCache>(json);
            if (cache == null) return false;
            if (DateTime.Now - cache.Timestamp > TimeSpan.FromHours(6))
                return false; // 缓存超6小时失效

            _domainIpMap = new Dictionary<string, IPAddress>(StringComparer.OrdinalIgnoreCase);
            foreach (var (domain, ipStr) in cache.Entries)
            {
                if (IPAddress.TryParse(ipStr, out var ip))
                    _domainIpMap[domain] = ip;
            }
            return _domainIpMap.Count > 0;
        }
        catch
        {
            return false;
        }
    }

    private static async Task<long> PingAsync(IPAddress ip, int timeoutMs = 3000)
    {
        try
        {
            using var ping = new Ping();
            var reply = await ping.SendPingAsync(ip, timeoutMs);
            return reply.Status == IPStatus.Success ? reply.RoundtripTime : -1;
        }
        catch
        {
            return -1;
        }
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }

    private sealed class IPAddressComparer : IEqualityComparer<IPAddress>
    {
        public static readonly IPAddressComparer Instance = new();
        public bool Equals(IPAddress? x, IPAddress? y) =>
            x?.Equals(y) ?? y == null;
        public int GetHashCode(IPAddress obj) =>
            obj.GetHashCode();
    }

    private sealed class IpCache
    {
        public DateTime Timestamp { get; set; }
        public Dictionary<string, string> Entries { get; set; } = new();
    }
}
