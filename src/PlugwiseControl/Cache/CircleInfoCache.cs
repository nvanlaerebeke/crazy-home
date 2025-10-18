using System;
using LanguageExt.Common;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PlugwiseControl.Message.Requests;
using PlugwiseControl.Message.Responses;

namespace PlugwiseControl.Cache;

internal sealed class CircleInfoCache {
    private const string CachePrefix = "CircleInfoCache_";

    private readonly IRequestManager _requestManager;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<CircleInfoCache> _logger;

    private record CachedRecord(CircleInfoResponse? CircleInfoResponse, int ErrorCount);

    public CircleInfoCache(IRequestManager requestManager, IMemoryCache memoryCache, ILogger<CircleInfoCache> logger) {
        _requestManager = requestManager;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public Result<CircleInfoResponse> Get(string mac) {
        // ReSharper disable once InvertIf
        if (_memoryCache.TryGetValue(GetKey(mac), out CachedRecord? cachedRecord)) {
            _logger.LogInformation("[{mac}]CircleInfo cache => {Info}", mac,
                cachedRecord?.CircleInfoResponse?.ToString());
            if (cachedRecord?.CircleInfoResponse != null) {
                return cachedRecord.CircleInfoResponse;
            }
        }

        return _requestManager.Send<CircleInfoResponse>(new CircleInfoRequest(mac)).Match(
            response => {
                _memoryCache.Set(GetKey(mac), new CachedRecord(response, 0), Settings.CircleInfoCacheDuration);
                _logger.LogInformation("[{mac}]CircleInfo caching => {Info}", mac, response.ToString());
                return new Result<CircleInfoResponse>(response);
            },
            ex => {
                var errorCount = (cachedRecord?.ErrorCount ?? 0) + 1;
                var cachedTimeSpan = TimeSpan.FromTicks(TimeSpan.FromMinutes(1).Ticks * errorCount);
                _memoryCache.Set(GetKey(mac), new CachedRecord(null, errorCount), cachedTimeSpan);
                _logger.LogError(ex, "[{mac}]CircleInfo error => {Info}", mac, ex.Message);
                return new Result<CircleInfoResponse>(ex);
            });
    }

    public void Invalidate(string mac) {
        _memoryCache.Remove(GetKey(mac));
    }

    private static string GetKey(string mac) => $"{CachePrefix}{mac}";
}
