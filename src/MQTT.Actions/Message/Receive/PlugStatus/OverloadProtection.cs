using System.Text.Json.Serialization;

namespace MQTT.Actions.Message.Receive.PlugStatus;

internal sealed class OverloadProtection {
    [JsonPropertyName("enable_max_voltage")]
    public required string EnableMaxVoltage { get; init; }

    [JsonPropertyName("enable_min_current")]
    public required string EnableMinCurrent { get; init; }

    [JsonPropertyName("enable_min_power")]
    public required string EnableMinPower { get; init; }

    [JsonPropertyName("enable_min_voltage")]
    public required string EnableMinVoltage { get; init; }

    [JsonPropertyName("max_current")]
    public required double MaxCurrent { get; init; }

    [JsonPropertyName("max_power")]
    public required double MaxPower { get; init; }

    [JsonPropertyName("max_voltage")]
    public required double MaxVoltage { get; init; }

    [JsonPropertyName("min_current")]
    public required double MinCurrent { get; init; }

    [JsonPropertyName("min_power")]
    public required double MinPower { get; init; }

    [JsonPropertyName("min_voltage")]
    public required double MinVoltage { get; init; }
}
