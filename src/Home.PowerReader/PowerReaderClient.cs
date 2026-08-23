using System.Net.Http.Json;
using Home.Config;
using Microsoft.Extensions.Logging;

namespace Home.PowerReader;

internal interface IPowerReaderClient {
    Task<DataRecord?> GetAsync(CancellationToken? ct);
}

internal sealed class PowerReaderClient : IPowerReaderClient {
    private readonly HttpClient _http;
    private readonly ISettings _settings;
    private readonly ILogger<PowerReaderClient> _logger;

    public PowerReaderClient(HttpClient http, ISettings settings, ILogger<PowerReaderClient> logger) {
        _http = http;
        _settings = settings;
        _logger = logger;
    }

    public async Task<DataRecord?> GetAsync(CancellationToken? ct) {
        var uri = _settings.Power.MetricsUri;
        
        if (uri is null) {
            _logger.LogWarning("No metrics uri configured for PowerReader, skipping");
            return null;
        }
        
        return await _http.GetFromJsonAsync<DataRecord>(uri, cancellationToken: ct ?? CancellationToken.None);
    }
}
