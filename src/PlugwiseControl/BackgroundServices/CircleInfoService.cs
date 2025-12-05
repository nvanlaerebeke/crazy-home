using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PlugwiseControl.Cache;

namespace PlugwiseControl.BackgroundServices;

internal sealed class CircleInfoService : BackgroundService {
    private readonly CircleInfoCache _circleInfoCache;
    private readonly ILogger<CircleInfoService> _logger;

    public CircleInfoService(
        CircleInfoCache circleInfoCache,
        ILogger<CircleInfoService> logger
    ) {
        _circleInfoCache = circleInfoCache;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        // PeriodicTimer is more efficient and cancellation-friendly than Task.Delay loops
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(10));
        _logger.LogInformation("CircleInfo Caching Service Started");

        try {
            while (await timer.WaitForNextTickAsync(stoppingToken)) {
                await RefreshCache(stoppingToken);
            }
        } catch (OperationCanceledException) {
            // normal on shutdown
        }
        finally {
            _logger.LogInformation("CircleInfo Caching Service Stopped");
        }
    }

    private async Task RefreshCache(CancellationToken ct) {
        foreach (var mac in Settings.CachedMacAddresses) {
            if (ct.IsCancellationRequested) {
                return;
            }

            _circleInfoCache.Invalidate(mac);
            _circleInfoCache.Get(mac);

            await Task.Delay(TimeSpan.FromSeconds(5), ct);
        }
    }
}
