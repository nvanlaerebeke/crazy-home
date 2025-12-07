using Home.Db;
using Home.Error;
using Microsoft.EntityFrameworkCore;
using MQTT.Actions.Cache;

namespace MQTT.Actions.Actions;

internal sealed class SetAllowChangeState {
    private readonly DeviceCache _cache;
    private readonly HomeDbContextFactory _dbContextFactory;
    
    public SetAllowChangeState(DeviceCache cache, HomeDbContextFactory dbContextFactory) {
        _cache = cache;
        _dbContextFactory = dbContextFactory;
    }

    public async Task<bool> ExecuteAsync(string id, bool allowStateChange) {
        await using var work = await _dbContextFactory.GetAsync();
        var device = await work.Devices.FirstOrDefaultAsync(x => x.IeeeAddress.Equals(id));
        if (device is null) {
            throw HomeApiException.from(ApiErrorCode.NotFound);
        }

        if (device.AllowStateChange == allowStateChange) {
            return true;
        }

        device.AllowStateChange = allowStateChange;
        work.Devices.Update(device);
        await work.SaveChangesAsync();
        
        _cache.UpdateDevice(device);
        return true;
    }
}
