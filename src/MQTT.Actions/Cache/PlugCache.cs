using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MQTT.Actions.Message;

namespace MQTT.Actions.Cache;

internal sealed class PlugCache {
    private const string CachePrefix = "MQTT_PLUG_CACHE_";
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);
    
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<PlugCache> _logger;

    private record CachedRecord(PlugStatus? PlugStatus, int ErrorCount);

    public PlugCache(IMemoryCache memoryCache, ILogger<PlugCache> logger) {
        _memoryCache = memoryCache;
        _logger = logger;
    }

    /*public Result<CircleInfoResponse> Get(string mac) {
        // ReSharper disable once InvertIf
        if (_memoryCache.TryGetValue(GetKey(mac), out CachedRecord? cachedRecord)) {
            _logger.LogInformation("[{mac}]CircleInfo cache => {Info}", mac,
                cachedRecord?.CircleInfoResponse?.ToString());
            if (cachedRecord?.CircleInfoResponse != null) {
                return cachedRecord.CircleInfoResponse;
            }
        }

        return _requestManager.Send<CircleInfoResponse>(new CircleInfoRequest(mac)).Match(
            response => {
                _memoryCache.Set(GetKey(mac), new CachedRecord(response, 0), Settings.CircleInfoCacheDuration);
                _logger.LogInformation("[{mac}]CircleInfo caching => {Info}", mac, response.ToString());
                return new Result<CircleInfoResponse>(response);
            },
            ex => {
                var errorCount = (cachedRecord?.ErrorCount ?? 0) + 1;
                var cachedTimeSpan = TimeSpan.FromTicks(TimeSpan.FromMinutes(1).Ticks * errorCount);
                _memoryCache.Set(GetKey(mac), new CachedRecord(null, errorCount), cachedTimeSpan);
                _logger.LogError(ex, "[{mac}]CircleInfo error => {Info}", mac, ex.Message);
                return new Result<CircleInfoResponse>(ex);
            });
    }*/

    public void Set(string identifier, PlugStatus plugStatus) {
        _memoryCache.Set(GetKey(identifier), new CachedRecord(plugStatus, 0), _cacheDuration);
    }
    public void Invalidate(string identifier) {
        _memoryCache.Remove(GetKey(identifier));
    }

    private static string GetKey(string identifier) => $"{CachePrefix}{identifier}";
}
