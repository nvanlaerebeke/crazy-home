using Home.Db;
using Microsoft.Extensions.Caching.Memory;
using MQTT.Actions.Objects;

namespace MQTT.Actions.Cache;
/// <summary>
/// Sensor Cache
///
/// Note: Sensor data does not expire, this is due to the way the sensors are implemented.
///       A sensor only sends data when it's being changed, as it is something battery operated.
/// 
///       Letting the cache expire would "remove" that last known value from the cache, so if no temperature
///       change was detected during CACHE INTERVAL, then the sensor is "lost"
/// </summary>
internal sealed class SensorCache {
    private const string CachePrefix = "MQTT_SENSOR_CACHE_";

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

    public SensorDto? Get(string id) {
        if (!_memoryCache.TryGetValue(GetKey(id), out SensorCacheEntry? cachedRecord) || cachedRecord is null) {
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
        _memoryCache.Set(GetKey(sensorStatus.Id), new SensorCacheEntry(sensorStatus.Id, sensorStatus, DateTime.Now));
    }

    private static string GetKey(string id) => $"{CachePrefix}{id}";

    public SensorCacheEntry? GetCacheEntry(string id) {
        if (!_memoryCache.TryGetValue(GetKey(id), out SensorCacheEntry? cachedRecord) || cachedRecord is null) {
            return null;
        }

        var device = _deviceCache.Get(id);
        if (device != null) {
            return new SensorCacheEntry(device.Id, new() {
                Id = device.IeeeAddress,
                Name = device.FriendlyName,
                Battery = cachedRecord.Sensor.Battery,
                Humidity = cachedRecord.Sensor.Battery,
                Temperature = cachedRecord.Sensor.Battery,
                LinkQuality = cachedRecord.Sensor.Battery,
            }, cachedRecord.LastUpdated);
            
        }
        return cachedRecord;
    }
}
