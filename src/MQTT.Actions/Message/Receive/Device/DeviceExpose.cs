using System.Text.Json.Serialization;

namespace MQTT.Actions.Message.Receive.Device;

internal sealed class DeviceExpose {
    [JsonPropertyName("type")]
    public string? Type { get; init; }
    [JsonPropertyName("name")]
    public string? Name { get; init; }
    [JsonPropertyName("unit")]
    public string? Unit { get; init; }
}

