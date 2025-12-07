using Home.Db;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTT.Actions.Cache;
using MQTT.Actions.Message.Request;
// ReSharper disable ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator

namespace MQTT.Actions.Services;

/// <summary>
/// Auto-renew the devices caches every x seconds
///
/// The caches for each type are updated based on the device cache
/// </summary>
internal sealed class AutoRefreshService : BackgroundService {
    private readonly DeviceCache _deviceCache;
    private readonly PlugCache _plugCache;
    private readonly SensorCache _sensorCache;
    private readonly SwitchCache _switchCache;
    private readonly MqttClient _client;
    private readonly ILogger<AutoRefreshService> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(10);
    private readonly TimeSpan _expiryInterval = TimeSpan.FromMinutes(10);

    public AutoRefreshService(
        DeviceCache deviceCache,
        PlugCache plugCache,
        SensorCache sensorCache,
        SwitchCache switchCache, 
        MqttClient client,
        ILogger<AutoRefreshService> logger
    ) {
        _deviceCache = deviceCache;
        _plugCache = plugCache;
        _sensorCache = sensorCache;
        _switchCache = switchCache;
        _client = client;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        using var timer = new PeriodicTimer(_checkInterval);

        while (await timer.WaitForNextTickAsync(stoppingToken)) {
            try {
                await RefreshAllAsync();
            } catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) {
                /* normal shutdown */
            } catch (Exception ex) {
                _logger.LogError(ex, "Background Refresh Service Failure!");
            }
        }
    }

    private async Task RefreshAllAsync() {
        var plugs = _deviceCache.GetAll(DeviceType.Plug);
        foreach (var plug in plugs) {
            var cachedEntry = _plugCache.GetCacheEntry(plug.Id);
            if (cachedEntry == null || DateTime.Now - _expiryInterval <= cachedEntry.LastUpdated) {
                await _client.SendAsync(new GetDeviceInfo(plug.Id));
            }
        }
        
        var sensors = _deviceCache.GetAll(DeviceType.Sensor);
        foreach (var sensor in sensors) {
            var cachedEntry = _sensorCache.GetCacheEntry(sensor.Id);
            if (cachedEntry == null || DateTime.Now - _expiryInterval <= cachedEntry.LastUpdated) {
                await _client.SendAsync(new GetDeviceInfo(sensor.Id));
            }
        }
        
        var switches = _deviceCache.GetAll(DeviceType.Switch);
        foreach (var switchObj in switches) {
            var cachedEntry = _switchCache.GetCacheEntry(switchObj.Id);
            if (cachedEntry == null || DateTime.Now - _expiryInterval <= cachedEntry.LastUpdated) {
                await _client.SendAsync(new GetDeviceInfo(switchObj.Id));
            }
        }
    }
}
