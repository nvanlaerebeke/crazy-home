using MQTT.Actions.Cache;
using MQTT.Actions.Objects;

namespace MQTT.Actions.Actions.Switch;

internal sealed class GetAll {
    private readonly SwitchCache _cache;
   
    public GetAll(SwitchCache cache) {
        _cache = cache;
    }
    
    public Task<List<SwitchStatusDto>> ExecuteAsync() {
        return Task.FromResult(_cache.GetAll());
    }
}

