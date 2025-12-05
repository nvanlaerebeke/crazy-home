using System.Text.Json.Serialization;

namespace MQTT.Actions.Message.Receive.Bridge;

internal sealed class CoordinatorInfo {
    [JsonPropertyName("ieee_address")]
    public string IeeeAddress { get; init; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; init; } = string.Empty;

    [JsonPropertyName("meta")]
    public CoordinatorMeta Meta { get; init; } = new();
}
