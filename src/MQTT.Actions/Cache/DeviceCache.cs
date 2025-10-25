using Home.Config;
using Microsoft.Extensions.Caching.Memory;
using MQTT.Actions.Message.Receive.Device;
using MQTT.Actions.Objects;

namespace MQTT.Actions.Cache;

internal sealed class DeviceCache {
    private const string CachePrefix = "MQTT_DEVICE_CACHE";

    private readonly ISettings _settings;
    private readonly IMemoryCache _memoryCache;

    private record Device(DeviceType Type, string Id);

    public DeviceCache(ISettings settings, IMemoryCache memoryCache) {
        _settings = settings;
        _memoryCache = memoryCache;
    }

    public List<string> GetAll(DeviceType deviceType) {
        if (!_memoryCache.TryGetValue(GetKey(), out List<Device>? devices) || devices == null) {
            return [];
        }

        return devices.Where(d => d.Type == deviceType).Select(d => d.Id).ToList();
    }

    public void Set(List<DeviceDefinition> devices) {
        var deviceList = new List<Device>();

        foreach (var device in devices) {
            if (_settings.Mqtt.PlugModelIds.Contains(device.Model)) {
                deviceList.Add(new(DeviceType.Plug, device.FriendlyName));
            }

            if (_settings.Mqtt.SensorModelIds.Contains(device.Model)) {
                deviceList.Add(new(DeviceType.Sensor, device.FriendlyName));
            }
        }

        _memoryCache.Set(GetKey(), deviceList);
    }

    private static string GetKey() => CachePrefix;
}
