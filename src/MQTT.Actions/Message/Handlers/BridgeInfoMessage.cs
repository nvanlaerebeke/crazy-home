using System.Text.Json;
using Microsoft.Extensions.Logging;
using MQTT.Actions.Cache;
using MQTT.Actions.Message.Receive.Bridge;

namespace MQTT.Actions.Message.Handlers;

internal sealed class BridgeInfoMessage : IMessageHandler {
    private readonly BridgeCache _bridgeCache;
    private readonly ILogger<BridgeInfoMessage> _logger;

    public BridgeInfoMessage(BridgeCache bridgeCache, ILogger<BridgeInfoMessage> logger) {
        _bridgeCache = bridgeCache;
        _logger = logger;
    }

    public Task HandleAsync(string topic, string payload) {
        switch (topic) {
            case "zigbee2mqtt/bridge/state": {
                var bridgeState = JsonSerializer.Deserialize<BridgeState>(payload);
                if (bridgeState is null) {
                    _logger.LogWarning("Invalid bridge state received: {Payload}", payload);
                    return Task.CompletedTask;
                }

                _bridgeCache.SetState(bridgeState);
                break;
            }
            case "zigbee2mqtt/bridge/info":
                var bridgeInfo = JsonSerializer.Deserialize<BridgeInfo>(payload);
                if (bridgeInfo is null) {
                    _logger.LogWarning("Invalid bridge info received: {Payload}", payload);
                    return Task.CompletedTask;
                }

                _bridgeCache.SetInfo(bridgeInfo);
                break;
        }

        return Task.CompletedTask;
    }

    public bool AcceptsTopic(string topic)
        => topic.Equals("zigbee2mqtt/bridge/state") || topic.Equals("zigbee2mqtt/bridge/info");
}
