using Home.Db;
using Microsoft.Extensions.Caching.Memory;
using MQTT.Actions.Objects;

namespace MQTT.Actions.Cache;

internal sealed class PlugCache {
    private const string CachePrefix = "MQTT_PLUG_CACHE_";
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(15);

    private readonly IMemoryCache _memoryCache;
    private readonly DeviceCache _deviceCache;

    public record PlugCacheEntry(string Id, PlugStatusDto Status, DateTime LastUpdated);

    public PlugCache(IMemoryCache memoryCache, DeviceCache deviceCache) {
        _memoryCache = memoryCache;
        _deviceCache = deviceCache;
    }

    public List<PlugStatusDto> GetAll() {
        return _deviceCache.GetAll(DeviceType.Plug).Select(x => Get(x.IeeeAddress)).OfType<PlugStatusDto>().ToList();
    }

    public PlugStatusDto? Get(string id) {
        return GetCacheEntry(id)?.Status;
    }

    public List<PlugCacheEntry> GetCacheEntries() {
        return _deviceCache
            .GetAll(DeviceType.Plug)
            .Select(x => GetCacheEntry(x.IeeeAddress))
            .OfType<PlugCacheEntry>()
            .ToList();
    }

    public void Set(PlugStatusDto plugStatus) {
        _memoryCache.Set(GetKey(plugStatus.Id), new PlugCacheEntry(plugStatus.Id, plugStatus, DateTime.Now), _cacheDuration);
    }

    public PlugCacheEntry? GetCacheEntry(string id) {
        if (!_memoryCache.TryGetValue(GetKey(id), out PlugCacheEntry? cachedRecord) || cachedRecord is null) {
            return null;
        }

        return cachedRecord;
    }

    private static string GetKey(string identifier) => $"{CachePrefix}{identifier}";
}
