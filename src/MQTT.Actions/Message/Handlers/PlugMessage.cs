using System.Text.Json;
using Home.Db;
using Microsoft.Extensions.Logging;
using MQTT.Actions.Cache;
using MQTT.Actions.Objects.ExtensionMethods;

namespace MQTT.Actions.Message.Handlers;

internal sealed class PlugMessage: IMessageRouter {
    private readonly DeviceCache _deviceCache;
    private readonly PlugCache _plugCache;
    private readonly ILogger _logger;

    public PlugMessage(DeviceCache deviceCache, PlugCache plugCache, ILogger<PlugMessage> logger) {
        _deviceCache = deviceCache;
        _plugCache = plugCache;
        _logger = logger;
    }
    
    public Task RouteAsync(string topic, string payload) {
        _logger.LogInformation("Received plug message with topic {Topic}", topic);
        var device = _deviceCache.GetAll(DeviceType.Plug).FirstOrDefault(x => topic.EndsWith(x.IeeeAddress));
        if (device is null) {
            _logger.LogError("Trying to add plug status for unknown device: {DeviceId}", topic);
            return  Task.CompletedTask;
        }
        
        var plugStatus = JsonSerializer.Deserialize<PlugStatus>(payload);
        if (plugStatus is null) {
            return Task.CompletedTask;
        }
        _logger.LogInformation("Received plug ({Id}) information", device);
        _plugCache.Set(plugStatus.ToDto(device));
        return Task.CompletedTask;
    }

    public bool AcceptsTopic(string topic) {
        return _deviceCache.GetAll(DeviceType.Plug).Select(x => x.IeeeAddress).Any(topic.EndsWith);
    }
}

