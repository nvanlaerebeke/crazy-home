using System.Text.Json.Serialization;
using MQTT.Actions.Message.Receive.Device;

namespace MQTT.Actions.Message;

internal sealed class Zigbee2MqttDevice {
    [JsonPropertyName("ieee_address")]
    public required string IeeeAddress { get; init; }
    
    [JsonPropertyName("friendly_name")]
    public required string FriendlyName { get; init; }
    
    [JsonPropertyName("type")]
    public required string Type { get; init; }
    
    [JsonPropertyName("power_source")]
    public required string PowerSource { get; init; }
    
    [JsonPropertyName("definition")]
    public required DeviceDefinition Definition { get; init; } 
}

