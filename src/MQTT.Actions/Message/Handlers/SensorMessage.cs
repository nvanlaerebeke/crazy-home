using System.Text.Json;
using Home.Db;
using Microsoft.Extensions.Logging;
using MQTT.Actions.Cache;
using MQTT.Actions.Message.Receive.Sensor;
using MQTT.Actions.Objects.ExtensionMethods;

namespace MQTT.Actions.Message.Handlers;

internal sealed class SensorMessage: IMessageHandler  {
    private readonly SensorCache _sensorCache;
    private readonly DeviceCache _deviceCache;
    private readonly ILogger<SensorMessage> _logger;

    public SensorMessage(SensorCache sensorCache, DeviceCache deviceCache, ILogger<SensorMessage> logger) {
        _sensorCache = sensorCache;
        _deviceCache = deviceCache;
        _logger = logger;
    }
    public Task HandleAsync(string topic, string payload) {
        var device = _deviceCache.GetAll(DeviceType.Sensor).FirstOrDefault(x => topic.EndsWith(x.IeeeAddress));
        if (device is null) {
            _logger.LogError("Trying to add sensor status for unknown device: {DeviceId}", topic);
            return  Task.CompletedTask;
        }
        
        var sensorStatus = JsonSerializer.Deserialize<Sensor>(payload);
        if (sensorStatus is null) {
            _logger.LogError("Invalid sensor ({Identifier}) payload: {Payload}", topic, payload);
            return Task.CompletedTask;
        }
        
        _logger.LogInformation("Received sensor ({Id}) information", device.IeeeAddress);
        
        _sensorCache.Set(sensorStatus.ToDto(device));
        return Task.CompletedTask;
    }

    public bool AcceptsTopic(string topic) {
        return _deviceCache.GetAll(DeviceType.Sensor).Any(x => topic.EndsWith(x.IeeeAddress));
    }
}

