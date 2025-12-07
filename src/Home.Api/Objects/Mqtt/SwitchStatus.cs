using System.Text.Json.Serialization;
using Home.Db;

namespace Home.Api.Objects.Mqtt;

public sealed class SwitchStatus {
    [JsonPropertyName("id")]
    public required string Identifier { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("state")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required SwitchState SwitchState { get; init; }
    
    [JsonPropertyName("allow_state_change")]
    public bool AllowStateChange { get; set; }

    [JsonPropertyName("power_on_behaviour")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SwitchState PowerOnBehavior { get; set; }
}
