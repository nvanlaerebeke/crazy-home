using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTT.Actions.Cache;
using MQTT.Actions.Message.Request;

namespace MQTT.Actions.Services;

internal sealed class AutoRefreshService : BackgroundService {
    private readonly PlugCache _plugCache;
    private readonly SensorCache _sensorCache;
    private readonly MqttClient _client;
    private readonly ILogger<AutoRefreshService> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(10);
    private readonly TimeSpan _expiryInterval = TimeSpan.FromMinutes(10);

    public AutoRefreshService(
        PlugCache plugCache,
        SensorCache sensorCache,
        MqttClient client,
        ILogger<AutoRefreshService> logger
    ) {
        _plugCache = plugCache;
        _sensorCache = sensorCache;
        _client = client;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        using var timer = new PeriodicTimer(_checkInterval);

        while (await timer.WaitForNextTickAsync(stoppingToken)) {
            try {
                await RefreshPlugsAsync();
            } catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) {
                /* normal shutdown */
            } catch (Exception ex) {
                _logger.LogError(ex, "Background Refresh Service Failure!");
            }
        }
    }

    private async Task RefreshPlugsAsync() {
        var plugs = _plugCache.GetCacheEntries();
        foreach (var plug in plugs.Where(x => DateTime.Now - _expiryInterval <= x.LastUpdated)) {
            await _client.SendAsync(new GetDeviceInfo(plug.Id));
        }

        var sensors = _sensorCache.GetCacheEntries();
        foreach (var sensor in sensors.Where(x => DateTime.Now - _expiryInterval <= x.LastUpdated)) {
            await _client.SendAsync(new GetDeviceInfo(sensor.Id));
        }
    }
}
