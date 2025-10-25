using MQTT.Actions.Cache;
using MQTT.Actions.Objects;

namespace MQTT.Actions.Actions.Plug;

internal sealed class GetPlugStatus {
    private readonly PlugCache _cache;
    public GetPlugStatus(PlugCache cache) {
        _cache = cache;
    }
    public Task<PlugStatusDto?> ExecuteAsync(string id) {
        return Task.FromResult(_cache.Get(id));
    }
}

