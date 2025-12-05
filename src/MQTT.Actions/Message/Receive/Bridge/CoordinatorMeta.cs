using System.Text.Json.Serialization;

namespace MQTT.Actions.Message.Receive.Bridge;

internal sealed class CoordinatorMeta {
    [JsonPropertyName("major")]
    public int MajorRel { get; init; }

    [JsonPropertyName("minor")]
    public int Minor { get; init; }
    
    [JsonPropertyName("build")]
    public int Build { get; init; }

    [JsonPropertyName("patch")]
    public int Patch { get; init; }

    [JsonPropertyName("revision")]
    public string Revision { get; init; } = string.Empty;
}
