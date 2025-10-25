using MQTT.Actions.Cache;
using MQTT.Actions.Objects;

namespace MQTT.Actions.Actions.Plug;

internal sealed class GetAll {
    private readonly PlugCache _cache;
   
    public GetAll(PlugCache cache) {
        _cache = cache;
    }
    
    public Task<List<PlugStatusDto>> ExecuteAsync() {
        return Task.FromResult(_cache.GetAll());
    }
}

