using System.Text.Json.Serialization;

namespace MQTT.Actions.Message.Receive.Sensor;

internal sealed class UpdateInfo {
    [JsonPropertyName("installed_version")]
    public int? InstalledVersion { get; set; }

    [JsonPropertyName("latest_version")]
    public int? LatestVersion { get; set; }

    // e.g., "available", "up_to_date", or null
    [JsonPropertyName("state")]
    public string? State { get; set; }
}
