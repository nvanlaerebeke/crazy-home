using System.Text.Json.Serialization;

namespace MQTT.Actions.Message.Receive.Device;

internal sealed class DeviceDefinition {
    [JsonPropertyName("vendor")]
    public string Vendor { get; init; } = string.Empty;
    
    [JsonPropertyName("manufacturer")]
    public string Manufacturer { get; init; } = string.Empty;

    [JsonPropertyName("model_id")]
    public string Model { get; init; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; init; } = string.Empty;

    [JsonPropertyName("exposes")]
    public List<DeviceExpose> Exposes { get; init; } = [];

    [JsonPropertyName("friendly_name")]
    public string FriendlyName { get; init; } = string.Empty;

    [JsonPropertyName("ieee_address")]
    public string IeeeAddress { get; init; } = string.Empty;
}
