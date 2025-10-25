using System.Text.Json.Serialization;

namespace MQTT.Actions.Message;

public sealed class UpdateInfo {
    [JsonPropertyName("installed_version")]
    public required int InstalledVersion { get; init; }

    [JsonPropertyName("latest_version")]
    public required int LatestVersion { get; init; }

    [JsonPropertyName("state")]
    public required string State { get; init; }
}
