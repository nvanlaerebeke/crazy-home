using System.Text.Json.Serialization;

namespace MQTT.Actions.Message.Receive.Bridge;

internal sealed class ModuleVersion {
    [JsonPropertyName("version")]
    public string Version { get; init; } = "";
}
