using System.Text.Json.Serialization;

namespace Home.Api.Objects.Plugwise;

public class Calibration {
    [JsonPropertyName("mac")] public string Mac { get; set; } = string.Empty;

    [JsonPropertyName("gain_a")] public float GainA { get; set; }

    [JsonPropertyName("gain_b")] public float GainB { get; set; }

    [JsonPropertyName("offset_total")] public float OffsetTotal { get; set; }

    [JsonPropertyName("offset_noise")] public float OffsetNoise { get; set; }
}
