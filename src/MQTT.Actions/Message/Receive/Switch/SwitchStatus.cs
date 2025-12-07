using System.Text.Json.Serialization;

namespace MQTT.Actions.Message;

internal sealed class SwitchStatus {
    [JsonPropertyName("delayed_power_on_state")]
    public bool DelayedPowerOnState { get; init; }

    [JsonPropertyName("delayed_power_on_time")]
    public double DelayedPowerOnTime { get; init; }

    [JsonPropertyName("detach_relay_mode")]
    public bool DetachRelayMode { get; init; }

    [JsonPropertyName("external_trigger_mode")]
    public string ExternalTriggerMode { get; init; } = string.Empty;

    [JsonPropertyName("network_indicator")]
    public bool NetworkIndicator { get; init; }

    [JsonPropertyName("turbo_mode")]
    public bool TurboMode { get; init; }

    [JsonPropertyName("linkquality")]
    public int LinkQuality { get; init; }

    [JsonPropertyName("state")]
    public string State { get; init; } = string.Empty;
}
