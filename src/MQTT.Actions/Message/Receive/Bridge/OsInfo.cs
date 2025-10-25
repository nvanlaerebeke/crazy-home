using System.Text.Json.Serialization;

namespace MQTT.Actions.Message.Receive.Bridge;

internal sealed class OsInfo {
    // e.g. "Linux - 0.0.1 - x64"
    [JsonPropertyName("version")]
    public string Version { get; init; } = "";

    [JsonPropertyName("node_version")]
    public string NodeVersion { get; init; } = "";

    [JsonPropertyName("cpus")]
    public string Cpus { get; init; } = "";

    [JsonPropertyName("memory_mb")]
    public int MemoryMb { get; init; }
}
