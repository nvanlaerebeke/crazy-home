using System.Text.Json;
using Home.Db;
using Microsoft.Extensions.Logging;
using MQTT.Actions.Cache;
using MQTT.Actions.Objects.ExtensionMethods;

namespace MQTT.Actions.Message.Handlers;

internal sealed class SwitchMessage: IMessageHandler  {
    private readonly SwitchCache _switchCache;
    private readonly DeviceCache _deviceCache;
    private readonly ILogger<SwitchMessage> _logger;

    public SwitchMessage(SwitchCache switchCache, DeviceCache deviceCache, ILogger<SwitchMessage> logger) {
        _switchCache = switchCache;
        _deviceCache = deviceCache;
        _logger = logger;
    }
    public Task HandleAsync(string topic, string payload) {
        var device = _deviceCache.GetAll(DeviceType.Switch).FirstOrDefault(x => topic.EndsWith(x.IeeeAddress));
        if (device is null) {
            _logger.LogError("Trying to add switch status for unknown device: {DeviceId}", topic);
            return  Task.CompletedTask;
        }
        
        var switchObj = JsonSerializer.Deserialize<SwitchStatus>(payload);
        if (switchObj is null) {
            _logger.LogError("Invalid sensor ({Identifier}) payload: {Payload}", topic, payload);
            return Task.CompletedTask;
        }
        
        _logger.LogInformation("Received sensor ({Id}) information", device.IeeeAddress);
        
        _switchCache.Set(switchObj.ToDto(device));
        return Task.CompletedTask;
    }

    public bool AcceptsTopic(string topic) {
        return _deviceCache.GetAll(DeviceType.Switch).Any(x => topic.EndsWith(x.IeeeAddress));
    }
}

