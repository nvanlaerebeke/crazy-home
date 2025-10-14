using System.Text.Json.Serialization;

namespace Home.Api.Objects.Plugwise;

public class Calibration {
    [JsonPropertyName("mac")] public required string Mac { get; init; } = string.Empty;

    [JsonPropertyName("gain_a")] public float GainA { get; init; }

    [JsonPropertyName("gain_b")] public float GainB { get; init; }

    [JsonPropertyName("offset_total")] public float OffsetTotal { get; init; }

    [JsonPropertyName("offset_noise")] public float OffsetNoise { get; init; }
}
