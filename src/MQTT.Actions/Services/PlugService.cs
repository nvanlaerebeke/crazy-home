using Home.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTT.Actions.Cache;

namespace MQTT.Actions.Services;

internal sealed class PlugService : BackgroundService {
    private readonly PlugCache _plugCache;
    private readonly ISettings _settings;
    private readonly ILogger<PlugService> _logger;

    public PlugService(
        PlugCache plugCache,
        ISettings settings,
        ILogger<PlugService> logger
    ) {
        _plugCache = plugCache;
        _settings = settings;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        // PeriodicTimer is more efficient and cancellation-friendly than Task.Delay loops
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(10));
        _logger.LogInformation("MQTT Plug Caching Service Started");

        try {
            while (await timer.WaitForNextTickAsync(stoppingToken)) {
                await RefreshCache(stoppingToken);
            }
        } catch (OperationCanceledException) {
            // normal on shutdown
        }
        finally {
            _logger.LogInformation("MQTT Plug Caching Service Stopped");
        }
    }

    private async Task RefreshCache(CancellationToken ct) {
        foreach (var plug in _settings.Mqtt.Plugs.Where(x => x.SourceType == SourceType.Zigbee)) {
            var identifier = plug.Identifier;
            if (ct.IsCancellationRequested) {
                return;
            }

            _plugCache.Invalidate(identifier);
            //_plugCache.Get(identifier);

            await Task.Delay(TimeSpan.FromSeconds(5), ct);
        }
    }
}
