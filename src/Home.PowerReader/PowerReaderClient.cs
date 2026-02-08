using System.Net.Http.Json;
using Home.Config;

namespace Home.PowerReader;

internal interface IPowerReaderClient {
    Task<DataRecord?> GetAsync(CancellationToken? ct);
}

internal sealed class PowerReaderClient : IPowerReaderClient {
    private readonly HttpClient _http;
    private readonly ISettings _settings;

    public PowerReaderClient(HttpClient http, ISettings settings) {
        _http = http;
        _settings = settings;
    }

    public async Task<DataRecord?> GetAsync(CancellationToken? ct) {
        var uri = _settings.Power.MetricsUri;
        if (uri is null) {
            return null;
        }
        
        return await _http.GetFromJsonAsync<DataRecord>(uri, cancellationToken: ct ?? CancellationToken.None);
    }
}
