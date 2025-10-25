using MQTT.Actions.Cache;
using MQTT.Actions.Objects;

namespace MQTT.Actions.Actions.Sensor;

internal sealed class GetAll {
    private readonly SensorCache _sensorCache;

    public GetAll(SensorCache sensorCache) {
        _sensorCache = sensorCache;
    }
    public Task<List<SensorDto>> ExecuteAsync() {
        return Task.FromResult(_sensorCache.GetAll());
    }
}

