namespace Home.AutoPlayer.Actions.ZeroConf;

using System.Net.Http.Json;
using Zeroconf;

public sealed record SpotifyConnectLanDevice(
    string Name,
    string Ip,
    int Port,
    string CPath,
    string? DeviceId,
    string? RemoteName);

public static class SpotifyConnectLanDiscovery {
    // Typical DNS-SD service type for Spotify Connect
    private const string ServiceType = "_spotify-connect._tcp.local.";

    public static async Task<IReadOnlyList<SpotifyConnectLanDevice>> DiscoverAsync(TimeSpan timeout) {
        using var cts = new CancellationTokenSource(timeout);

        // ResolveAsync browses mDNS and returns discovered hosts/services
        var hosts = await ZeroconfResolver.ResolveAsync(ServiceType, cancellationToken: cts.Token);

        var results = new List<SpotifyConnectLanDevice>();

        foreach (var host in hosts) {
            // host.IPAddress is usually a string; sometimes multiple IPs exist
            var ip = host.IPAddress;

            foreach (var svc in host.Services.Values) {
                var port = svc.Port;

                // TXT records can be split across dictionaries
                // "CPath" is commonly present and points to the ZeroConf base path
                var txt = svc.Properties.SelectMany(p => p).ToDictionary(kv => kv.Key, kv => kv.Value);

                txt.TryGetValue("CPath", out var cpath);
                if (string.IsNullOrWhiteSpace(cpath))
                    cpath = "/"; // fallback; many implementations still answer getInfo on "/"

                // Try calling local ZeroConf API to getInfo
                var info = await TryGetInfoAsync(ip, port, cpath, cts.Token);

                results.Add(new SpotifyConnectLanDevice(
                    Name: host.DisplayName,
                    Ip: ip,
                    Port: port,
                    CPath: cpath,
                    DeviceId: info?.DeviceId,
                    RemoteName: info?.RemoteName
                ));
            }
        }

        return results;
    }

    private sealed record GetInfoResponse(string? deviceID, string? remoteName);

    private static async Task<(string? DeviceId, string? RemoteName)?> TryGetInfoAsync(
        string ip, int port, string cpath, CancellationToken ct) {
        // Spotify ZeroConf API uses HTTP calls like: <cpath>?action=getInfo :contentReference[oaicite:3]{index=3}
        var basePath = cpath.StartsWith("/") ? cpath : "/" + cpath;
        var url = $"http://{ip}:{port}{basePath}?action=getInfo";

        try {
            using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(2) };
            var resp = await http.GetFromJsonAsync<GetInfoResponse>(url, cancellationToken: ct);
            return (resp?.deviceID, resp?.remoteName);
        } catch {
            return null;
        }
    }
}
