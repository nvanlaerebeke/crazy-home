using Microsoft.Extensions.Caching.Memory;
using MQTT.Actions.Objects;

namespace MQTT.Actions.Cache;

internal sealed class PlugCache {
    private const string CachePrefix = "MQTT_PLUG_CACHE_";
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);

    private readonly IMemoryCache _memoryCache;
    private readonly DeviceCache _deviceCache;

    public PlugCache(IMemoryCache memoryCache, DeviceCache deviceCache) {
        _memoryCache = memoryCache;
        _deviceCache = deviceCache;
    }

    public List<PlugStatusDto> GetAll() {
        return _deviceCache.GetAll(DeviceType.Plug).Select(Get).OfType<PlugStatusDto>().ToList();
    }

    public PlugStatusDto? Get(string identifier) {
        if (!_memoryCache.TryGetValue(GetKey(identifier), out PlugStatusDto? cachedRecord) || cachedRecord is null) {
            return null;
        }
        return cachedRecord;
    }

    public void Set(PlugStatusDto plugStatus) {
        _memoryCache.Set(GetKey(plugStatus.Identifier), plugStatus, _cacheDuration);
    }

    public void Invalidate(string identifier) {
        _memoryCache.Remove(GetKey(identifier));
    }

    private static string GetKey(string identifier) => $"{CachePrefix}{identifier}";
}
