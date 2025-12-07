using MQTT.Actions.Cache;
using MQTT.Actions.Objects;

namespace MQTT.Actions.Actions.Switch;

internal sealed class GetSwitchStatus {
    private readonly SwitchCache _cache;
    public GetSwitchStatus(SwitchCache cache) {
        _cache = cache;
    }
    public Task<SwitchStatusDto?> ExecuteAsync(string id) {
        return Task.FromResult(_cache.Get(id));
    }
}

