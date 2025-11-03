using Home.Config;
using Home.Db;
using Home.Db.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MQTT.Actions.Message.Receive.Device;
using MQTT.Actions.Message.Request;

namespace MQTT.Actions.Cache;

internal sealed class DeviceCache {
    private const string CachePrefix = "MQTT_DEVICE_CACHE";

    private readonly ISettings _settings;
    private readonly MqttClient _client;
    private readonly IMemoryCache _memoryCache;
    private readonly HomeDbContextFactory _dbContextFactory;
    private readonly ILogger<DeviceCache> _logger;

    private record DeviceCacheEntry(DeviceType Type, Device Device);

    public DeviceCache(
        ISettings settings,
        MqttClient client,
        IMemoryCache memoryCache,
        HomeDbContextFactory dbContextFactory,
        ILogger<DeviceCache> logger
    ) {
        _settings = settings;
        _client = client;
        _memoryCache = memoryCache;
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }

    public List<Device> GetAll(DeviceType deviceType) {
        if (!_memoryCache.TryGetValue(GetKey(), out List<DeviceCacheEntry>? devices) || devices == null) {
            return [];
        }

        return devices.Where(d => d.Type == deviceType).Select(x => x.Device).ToList();
    }

    public async Task SetAsync(List<DeviceDefinition> devices) {
        var deviceList = new List<DeviceCacheEntry>();

        foreach (var device in devices) {
            if (_settings.Mqtt.PlugModelIds.Contains(device.Model)) {
                _logger.LogInformation("Adding plug {Id}", device.FriendlyName);
                var plug = await AddToDbAsync(DeviceType.Plug, device);
                deviceList.Add(new(DeviceType.Plug, plug));
            }

            // ReSharper disable once InvertIf
            if (_settings.Mqtt.SensorModelIds.Contains(device.Model)) {
                _logger.LogInformation("Adding sensor {Id}", device.FriendlyName);
                var sensor = await AddToDbAsync(DeviceType.Sensor, device);
                deviceList.Add(new(DeviceType.Sensor, sensor));
            }
        }

        _memoryCache.Set(GetKey(), deviceList);
    }

    /// <summary>
    /// Update an existing cached device with the given Device object
    /// </summary>
    /// <param name="device"></param>
    public void UpdateDevice(Device device) {
        if (!_memoryCache.TryGetValue(GetKey(), out List<DeviceCacheEntry>? devices) || devices == null) {
            return;
        }

        for (int i = 0; i < devices.Count; i++) {
            if (devices[i].Device.IeeeAddress != device.IeeeAddress) {
                continue;
            }

            devices[i] = new DeviceCacheEntry(devices[i].Type, device);
        }

        _memoryCache.Set(GetKey(), devices);
    }

    /// <summary>
    /// Add a new device to the database
    /// 
    /// Note: if a device with the same ieeeAddress exists, nothing will be changed
    /// </summary>
    /// <param name="deviceType"></param>
    /// <param name="deviceDefinition"></param>
    /// <returns></returns>
    private async Task<Device> AddToDbAsync(DeviceType deviceType, DeviceDefinition deviceDefinition) {
        await using var work = await _dbContextFactory.GetAsync();

        //Update existing entry if needed
        var existingDevice =
            await work.Devices.FirstOrDefaultAsync(x => x.IeeeAddress.Equals(deviceDefinition.IeeeAddress)
            );

        if (existingDevice is not null) {
            //Make sure the Power On Behavior is set correctly!
            await _client.SendAsync(
                new SetPowerOnBehavior(existingDevice.IeeeAddress, existingDevice.PowerOnBehavior ?? SwitchState.On)
            );
            return existingDevice;
        }

        //Add new entry
        var device = new Device {
            IeeeAddress = deviceDefinition.IeeeAddress,
            FriendlyName = deviceDefinition.FriendlyName,
            PowerOnBehavior = SwitchState.On,
            AllowStateChange = true,
            DeviceType = deviceType,
        };
        work.Devices.Add(device);
        await work.SaveChangesAsync();
        return device;
    }

    private static string GetKey() => CachePrefix;
}
