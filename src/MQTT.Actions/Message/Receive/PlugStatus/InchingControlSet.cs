using System.Text.Json.Serialization;

namespace MQTT.Actions.Message.Receive.PlugStatus;

internal sealed class InchingControlSet {
    [JsonPropertyName("inching_control")]
    public required string InchingControl { get; init; }
}
