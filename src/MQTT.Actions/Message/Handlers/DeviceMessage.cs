using System.Text.Json;
using Microsoft.Extensions.Logging;
using MQTT.Actions.Cache;
using MQTT.Actions.Message.Receive.Device;

namespace MQTT.Actions.Message.Handlers;

internal sealed class DeviceMessage : IMessageRouter {
    private readonly DeviceCache _deviceCache;
    private readonly ILogger<DeviceMessage> _logger;

    public DeviceMessage(DeviceCache deviceCache, ILogger<DeviceMessage> logger) {
        _deviceCache = deviceCache;
        _logger = logger;
    }

    public async Task RouteAsync(string topic, string payload) {
        var devices = JsonSerializer.Deserialize<List<DeviceDefinition>>(payload);
        if (devices is null) {
            _logger.LogWarning("Invalid bridge state received: {Payload}", payload);
            return;
        }
        
        await _deviceCache.SetAsync(devices);
    }

    public bool AcceptsTopic(string topic) => topic.Equals("zigbee2mqtt/bridge/devices");
}
