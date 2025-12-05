using System.Text.Json.Serialization;
using Home.Db;

namespace Home.Api.Objects.Mqtt;

public sealed class Device {
    [JsonPropertyName("device_type")]
    public required DeviceType DeviceType { get; init; }
    
    [JsonPropertyName("ieeeAddress")]
    public required string IeeeAddress { get; init; }
    
    [JsonPropertyName("friendly_name")]
    public required string FriendlyName { get; init; }
}

