using System.Text.Json.Serialization;

namespace MQTT.Actions.Message.Receive.Bridge;

internal sealed class BridgeState {
    [JsonPropertyName("state")]
    public required string State { get; init; }
}

