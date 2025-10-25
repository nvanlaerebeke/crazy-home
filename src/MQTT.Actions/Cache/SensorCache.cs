using Microsoft.Extensions.Caching.Memory;
using MQTT.Actions.Objects;

namespace MQTT.Actions.Cache;

internal sealed class SensorCache {
    private const string CachePrefix = "MQTT_PLUG_CACHE_";
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);

    private readonly IMemoryCache _memoryCache;
    private readonly DeviceCache _deviceCache;

    public SensorCache(IMemoryCache memoryCache, DeviceCache deviceCache) {
        _memoryCache = memoryCache;
        _deviceCache = deviceCache;
    }

    public List<SensorDto> GetAll() {
        return _deviceCache.GetAll(DeviceType.Sensor).Select(Get).OfType<SensorDto>().ToList();
    }

    public SensorDto? Get(string identifier) {
        if (!_memoryCache.TryGetValue(GetKey(identifier), out SensorDto? cachedRecord) || cachedRecord is null) {
            return null;
        }
        return cachedRecord;
    }

    public void Set(SensorDto sensorStatus) {
        _memoryCache.Set(GetKey(sensorStatus.Id), sensorStatus, _cacheDuration);
    }

    public void Invalidate(string id) {
        _memoryCache.Remove(GetKey(id));
    }

    private static string GetKey(string id) => $"{CachePrefix}{id}";
}
