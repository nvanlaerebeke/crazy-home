using System.Text.Json.Serialization;

namespace Home.Api.Objects.Plugwise;

public class StickStatus {
    [JsonPropertyName("stick_mac")]
    public string StickMac { get; set; } = string.Empty;

    [JsonPropertyName("is_circle_plus")]
    public bool IsCirclePlus { get; set; }
}
