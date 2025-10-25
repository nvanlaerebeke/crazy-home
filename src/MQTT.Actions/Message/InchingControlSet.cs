using System.Text.Json.Serialization;

namespace MQTT.Actions.Message;

public sealed class InchingControlSet {
    [JsonPropertyName("inching_control")]
    public required string InchingControl { get; init; }
}
