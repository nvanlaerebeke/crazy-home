using System.Text.Json.Serialization;

namespace Home.Api.Objects.Mqtt;

internal sealed class SensorStatus {
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("battery")]
    public required int Battery { get; init; }

    [JsonPropertyName("humidity")]
    public required double Humidity { get; init; }

    [JsonPropertyName("temperature")]
    public required double Temperature { get; init; }

    [JsonPropertyName("link_quality")]
    public required int LinkQuality { get; init; }
}
