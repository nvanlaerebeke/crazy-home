using Home.Db;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using MQTT.Actions.Cache;
using MQTT.Actions.Objects;
using MQTT.Actions.Objects.ExtensionMethods;

namespace MQTT.Actions;

internal sealed class MqttDeviceActions : IMqttDeviceActions {
    private readonly DeviceCache _deviceCache;
    private readonly HomeDbContextFactory _dbContextFactory;

    public MqttDeviceActions(DeviceCache deviceCache, HomeDbContextFactory dbContextFactory) {
        _deviceCache = deviceCache;
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Result<List<DeviceDto>>> GetAllAsync() {
        await using var work = await _dbContextFactory.GetAsync();
        return work.Devices.Select(x => x.ToDto()).ToList();
    }

    public async Task<Result<bool>> SetFriendlyNameAsync(string ieeeAddress, string friendlyName) {
        try {
            await using var work = await _dbContextFactory.GetAsync();
            var device = await work.Devices.FirstOrDefaultAsync(x => x.IeeeAddress == ieeeAddress);
            if (device == null) {
                return new Result<bool>(new Exception("Device not found"));
            }

            device.FriendlyName = friendlyName;
            work.Devices.Update(device);
            await work.SaveChangesAsync();
            
            _deviceCache.UpdateDevice(device);
            
            return true;
        } catch (Exception ex) {
            return new Result<bool>(ex);
        }
    }

    public async Task<Result<bool>> RemoveAsync(string ieeeAddress) {
        await using var work = await _dbContextFactory.GetAsync();
        var device = work.Devices.FirstOrDefault(x =>x.IeeeAddress == ieeeAddress);
        if (device is null) {
            return true;
        }

        work.Devices.Remove(device);
        await work.SaveChangesAsync();
        return true;
    }
}
