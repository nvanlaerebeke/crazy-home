using Home.Db;
using Microsoft.Extensions.Caching.Memory;
using MQTT.Actions.Objects;

namespace MQTT.Actions.Cache;

internal sealed class SwitchCache {
    private const string CachePrefix = "MQTT_SWITCH_CACHE_";
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(15);

    private readonly IMemoryCache _memoryCache;
    private readonly DeviceCache _deviceCache;

    public record SwitchCacheEntry(string Id, SwitchStatusDto Switch, DateTime LastUpdated);

    public SwitchCache(IMemoryCache memoryCache, DeviceCache deviceCache) {
        _memoryCache = memoryCache;
        _deviceCache = deviceCache;
    }

    public List<SwitchStatusDto> GetAll() {
        var devices = _deviceCache.GetAll(DeviceType.Switch);
        var switches = devices.Select(x => Get(x.IeeeAddress)).OfType<SwitchStatusDto>().ToList();
        return switches;
    }

    public SwitchStatusDto? Get(string id) {
        return GetCacheEntry(id)?.Switch;
    }

    public List<SwitchCacheEntry> GetCacheEntries() {
        return _deviceCache
            .GetAll(DeviceType.Switch)
            .Select(x => GetCacheEntry(x.IeeeAddress))
            .OfType<SwitchCacheEntry>()
            .ToList();
    }

    public void Set(SwitchStatusDto status) {
        _memoryCache.Set(GetKey(status.Id), new SwitchCacheEntry(status.Id, status, DateTime.Now), _cacheDuration);
    }

    public SwitchCacheEntry? GetCacheEntry(string id) {
        if (!_memoryCache.TryGetValue(GetKey(id), out SwitchCacheEntry? cachedRecord) || cachedRecord is null) {
            return null;
        }

        var device = _deviceCache.Get(id);
        if (device != null) {
            return new SwitchCacheEntry(device.Id, new() {
                Id = device.IeeeAddress,
                Name = device.FriendlyName,
                SwitchState = cachedRecord.Switch.SwitchState,
                AllowStateChange = device.AllowStateChange,
                PowerOnBehavior = device.PowerOnBehavior ?? SwitchState.Off
            }, cachedRecord.LastUpdated);
        }
        return cachedRecord;
    }

    private static string GetKey(string identifier) => $"{CachePrefix}{identifier}";
}
