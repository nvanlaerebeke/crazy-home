using System.Text.Json.Serialization;

namespace Home.Api.Objects.Audio;

public sealed class AudioDevice {

    [JsonPropertyName("id")]
    public required string Id { get; init; }
    [JsonPropertyName("isActive")]
    public required bool IsActive { get; init; }
    [JsonPropertyName("isPrivateSession")]
    public required bool IsPrivateSession { get; init; }
    [JsonPropertyName("isRestricted")]
    public required bool IsRestricted { get; init; }
    [JsonPropertyName("name")]
    public required string Name {get; init; }
    [JsonPropertyName("supportsVolume")]
    public required bool SupportsVolume { get; init; }
    [JsonPropertyName("type")]
    public required string Type { get; init; }
    [JsonPropertyName("volumePercent")]
    public required int? VolumePercent { get; init; }
}

