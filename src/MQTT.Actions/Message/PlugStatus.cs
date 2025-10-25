#nullable enable
using System.Text.Json.Serialization;

namespace MQTT.Actions.Message;

public sealed class PlugStatus {
    [JsonPropertyName("current")]
    public required double Current { get; init; }

    [JsonPropertyName("energy_month")]
    public required double EnergyMonth { get; init; }

    [JsonPropertyName("energy_today")]
    public required double EnergyToday { get; init; }

    [JsonPropertyName("energy_yesterday")]
    public required double EnergyYesterday { get; init; }

    [JsonPropertyName("inching_control_set")]
    public required InchingControlSet InchingControlSet { get; init; }

    [JsonPropertyName("linkquality")]
    public required int Linkquality { get; init; }

    [JsonPropertyName("outlet_control_protect")]
    public required bool OutletControlProtect { get; init; }

    [JsonPropertyName("overload_protection")]
    public required OverloadProtection OverloadProtection { get; init; }

    [JsonPropertyName("power")]
    public required double Power { get; init; }

    [JsonPropertyName("power_on_behavior")]
    public required string PowerOnBehavior { get; init; }

    [JsonPropertyName("state")]
    public required string State { get; init; }

    [JsonPropertyName("update")]
    public required UpdateInfo Update { get; init; }

    [JsonPropertyName("voltage")]
    public required double Voltage { get; init; }
}
