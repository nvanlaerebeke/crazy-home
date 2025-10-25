using System.Text.Json.Serialization;

namespace MQTT.Actions.Message.Receive.Bridge;

internal sealed class NetworkInfo {
    [JsonPropertyName("channel")]
    public int Channel { get; init; }

    [JsonPropertyName("pan_id")]
    public int PanId { get; init; }

    [JsonPropertyName("extended_pan_id")]
    public string ExtendedPanId { get; init; } = string.Empty;
}
