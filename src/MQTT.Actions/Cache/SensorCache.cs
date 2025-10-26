using Home.Db;
using Microsoft.Extensions.Caching.Memory;
using MQTT.Actions.Objects;

namespace MQTT.Actions.Cache;

internal sealed class SensorCache {
    private const string CachePrefix = "MQTT_SENSOR_CACHE_";
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);

    private readonly IMemoryCache _memoryCache;
    private readonly DeviceCache _deviceCache;

    public record SensorCacheEntry(string Id, SensorDto Sensor, DateTime LastUpdated);

    public SensorCache(IMemoryCache memoryCache, DeviceCache deviceCache) {
        _memoryCache = memoryCache;
        _deviceCache = deviceCache;
    }

    public List<SensorDto> GetAll() {
        return _deviceCache.GetAll(DeviceType.Sensor).Select(x => Get(x.IeeeAddress)).OfType<SensorDto>().ToList();
    }

    public SensorDto? Get(string identifier) {
        if (!_memoryCache.TryGetValue(GetKey(identifier), out SensorCacheEntry? cachedRecord) || cachedRecord is null) {
            return null;
        }

        return cachedRecord.Sensor;
    }

    public List<SensorCacheEntry> GetCacheEntries() {
        return _deviceCache
            .GetAll(DeviceType.Plug)
            .Select(x => GetCacheEntry(x.IeeeAddress))
            .OfType<SensorCacheEntry>()
            .ToList();
    }

    public void Set(SensorDto sensorStatus) {
        _memoryCache.Set(GetKey(sensorStatus.Id), new SensorCacheEntry(sensorStatus.Id, sensorStatus, DateTime.Now),
            _cacheDuration);
    }

    private static string GetKey(string id) => $"{CachePrefix}{id}";

    private SensorCacheEntry? GetCacheEntry(string id) {
        if (!_memoryCache.TryGetValue(GetKey(id), out SensorCacheEntry? cachedRecord) || cachedRecord is null) {
            return null;
        }

        return cachedRecord;
    }
}
