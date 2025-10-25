using System.Text.Json.Serialization;
using Home.Shared;

namespace Home.Api.Objects.Mqtt;

public sealed class PlugStatus {
    [JsonPropertyName("id")]
    public required string Identifier { get; init; }
    
    [JsonPropertyName("name")]
    public required string Name { get; init; }
    
    [JsonPropertyName("state")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required SwitchState SwitchState { get; init; }
    
    [JsonPropertyName("usage")]
    public required double Usage { get; init; }
    
    [JsonPropertyName("unit")]
    public required string Unit { get; init; }

    [JsonPropertyName("current")]
    public double Current { get; set; }

    [JsonPropertyName("voltage")]
    public double Voltage { get; set; }

    [JsonPropertyName("power_factor")]
    public double PowerFactor { get; set; }
}

