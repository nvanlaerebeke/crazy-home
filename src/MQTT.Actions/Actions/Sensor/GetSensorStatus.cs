using MQTT.Actions.Cache;
using MQTT.Actions.Objects;

namespace MQTT.Actions.Actions.Sensor;

internal sealed class GetSensorStatus {
    private readonly SensorCache _sensorCache;
    public GetSensorStatus(SensorCache sensorCache) {
        _sensorCache = sensorCache;
    }
    public Task<SensorDto?> ExecuteAsync(string id) {
        return Task.FromResult(_sensorCache.Get(id)); 
    }
}

