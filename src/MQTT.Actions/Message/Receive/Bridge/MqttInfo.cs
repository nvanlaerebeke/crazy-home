using System.Text.Json.Serialization;

namespace MQTT.Actions.Message.Receive.Bridge;

internal sealed class MqttInfo {
    // e.g. "mqtt://localhost:1883"
    [JsonPropertyName("server")]
    public string Server { get; init; } = "";

    [JsonPropertyName("version")]
    public int Version { get; init; }
}
