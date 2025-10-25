using System.Text.Json;
using Microsoft.Extensions.Logging;
using MQTT.Actions.Cache;
using MQTT.Actions.Message.Receive.Sensor;
using MQTT.Actions.Objects;
using MQTT.Actions.Objects.ExtensionMethods;

namespace MQTT.Actions.Message.Handlers;

internal sealed class SensorMessage: IMessageRouter  {
    private readonly SensorCache _sensorCache;
    private readonly DeviceCache _deviceCache;
    private readonly ILogger<SensorMessage> _logger;

    public SensorMessage(SensorCache sensorCache, DeviceCache deviceCache, ILogger<SensorMessage> logger) {
        _sensorCache = sensorCache;
        _deviceCache = deviceCache;
        _logger = logger;
    }
    public Task RouteAsync(string topic, string payload) {
        var id = _deviceCache.GetAll(DeviceType.Sensor).FirstOrDefault(topic.EndsWith);
        if (string.IsNullOrEmpty(id)) {
            _logger.LogError("Trying to add plug status for unknown device: {DeviceId}", topic);
            return  Task.CompletedTask;
        }
        
        var sensorStatus = JsonSerializer.Deserialize<Sensor>(payload);
        if (sensorStatus is null) {
            return Task.CompletedTask;
        }
        
        _sensorCache.Set(sensorStatus.ToDto(id));
        return Task.CompletedTask;
    }

    public bool AcceptsTopic(string topic) {
        return _deviceCache.GetAll(DeviceType.Sensor).Any(topic.EndsWith);
    }
}

