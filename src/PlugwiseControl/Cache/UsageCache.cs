using System;
using LanguageExt.Common;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PlugwiseControl.Calibration;
using PlugwiseControl.Message;
using PlugwiseControl.Message.Requests;
using PlugwiseControl.Message.Responses;

namespace PlugwiseControl.Cache;

internal class UsageCache {
    private const string CachePrefix = "Usage_";
    private readonly Calibrator _calibrator;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<UsageCache> _logger;
    private readonly IRequestManager _requestManager;

    private record CacheRecord(double Usage, int ErrorCount);

    public UsageCache(
        IRequestManager requestManager, Calibrator calibrator, IMemoryCache memoryCache, ILogger<UsageCache> logger
    ) {
        _requestManager = requestManager;
        _calibrator = calibrator;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public double Get(string mac) {
        // ReSharper disable once InvertIf
        if (_memoryCache.TryGetValue(CachePrefix + mac, out CacheRecord? cachedUsage)) {
            _logger.LogInformation(
                "[{mac}]Cached values => Usage: {Usage}, ErrorCount: {ErrorCount}",
                mac,
                cachedUsage?.Usage,
                cachedUsage?.ErrorCount
            );

            if (cachedUsage is not null && cachedUsage.ErrorCount == 0) {
                return cachedUsage.Usage;
            }
        }

        return GetUsage(mac).Match(
            v => {
                _logger.LogInformation("[{mac}]Caching {Usage}", mac, v);
                _memoryCache.Set(GetKey(mac), new CacheRecord(v, 0), Settings.UsageCacheDuration);
                return v;
            },
            ex => {
                var cacheDuration = TimeSpan.FromTicks(
                    Settings.UsageCacheDuration.Ticks * (cachedUsage?.ErrorCount ?? 0) + 1
                );
                if (cacheDuration.TotalMinutes > 10) {
                    cacheDuration = TimeSpan.FromMinutes(10);
                }

                _memoryCache.Set(GetKey(mac), new CacheRecord(0, (cachedUsage?.ErrorCount ?? 0) + 1), cacheDuration);
                _logger.LogError(ex, "[{mac}]Error: {Message}", mac, ex.Message);
                return 0;
            });
    }

    private Result<double> GetUsage(string mac) {
        return _requestManager.Send<PowerUsageResponse>(new PowerUsageRequest(mac)).Match(
            v => {
                if (v.Status == Status.Success) {
                    try {
                        return _calibrator.GetCorrected(v.Pulse1, mac);
                    } catch (Exception ex) {
                        return new Result<double>(ex);
                    }
                }

                return new Result<double>(new Exception($"GetUsage request to {mac} not successful ({v.Status})"));
            },
            ex => new Result<double>(ex)
        );
    }

    private static string GetKey(string mac) => $"{CachePrefix}{mac}";
}
