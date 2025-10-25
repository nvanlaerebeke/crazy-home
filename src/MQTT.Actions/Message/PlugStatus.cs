using System.Text.Json.Serialization;
using MQTT.Actions.Message.Receive.PlugStatus;

namespace MQTT.Actions.Message;

internal sealed class PlugStatus {
    [JsonPropertyName("current")]
    public double Current { get; init; }

    [JsonPropertyName("energy_month")]
    public double EnergyMonth { get; init; }

    [JsonPropertyName("energy_today")]
    public double EnergyToday { get; init; }

    [JsonPropertyName("energy_yesterday")]
    public double EnergyYesterday { get; init; }

    [JsonPropertyName("inching_control_set")]
    public InchingControlSet? InchingControlSet { get; init; }

    [JsonPropertyName("linkquality")]
    public int LinkQuality { get; init; }

    [JsonPropertyName("outlet_control_protect")]
    public bool OutletControlProtect { get; init; }

    [JsonPropertyName("overload_protection")]
    public OverloadProtection? OverloadProtection { get; init; }

    [JsonPropertyName("power")]
    public double Power { get; init; }

    [JsonPropertyName("power_on_behavior")]
    public string PowerOnBehavior { get; init; } = string.Empty;

    [JsonPropertyName("state")]
    public string State { get; init; } = string.Empty;

    [JsonPropertyName("update")]
    public UpdateInfo? Update { get; init; }

    [JsonPropertyName("voltage")]
    public double Voltage { get; init; }
}
