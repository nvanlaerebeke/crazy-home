using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PlugwiseControl.Message;

namespace Home.Api.Objects.Plugwise;

public class CircleInfo {
    /// <summary>
    /// Mag address for the plug
    /// </summary>
    [JsonPropertyName("mac")]
    [Required(AllowEmptyStrings = false)]
    public required string Mac { get; init; }

    /// <summary>
    /// True if updating the state is allowed. (On/Off)
    /// </summary>
    [JsonPropertyName("allowStateUpdate")]
    public bool AllowStateUpdate { get; init; }

    /// <summary>
    /// Human-readable name for the plug
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Address of the Circle+ plug
    /// This is the gateway to all the other plugs
    /// The plugwise stick communicates only with this Circle
    /// </summary>
    [JsonPropertyName("circle_plus_mac")]
    public string CirclePlusMac { get; init; } = string.Empty;

    /// <summary>
    /// Current date init for the plug
    /// </summary>
    [JsonPropertyName("datetime")]
    public DateTime Date { get; init; }

    /// <summary>
    /// Current year init for the plug
    /// </summary>
    [JsonPropertyName("year")]
    public int Year { get; init; }

    /// <summary>
    /// Current month init for the plug
    /// </summary>
    [JsonPropertyName("month")]
    public int Month { get; init; }

    [JsonPropertyName("min")]
    public required string Min { get; init; }

    [JsonPropertyName("current_log")]
    public string CurrentLog { get; init; } = string.Empty;

    /// <summary>
    /// State of the plug, is it on or off
    /// </summary>
    [JsonPropertyName("switch_state")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SwitchState State { get; init; }

    [JsonPropertyName("hertz")]
    public string Hertz { get; init; } = string.Empty;

    [JsonPropertyName("hw1")]
    public string HW1 { get; init; } = string.Empty;

    [JsonPropertyName("hw2")]
    public string HW2 { get; init; } = string.Empty;

    [JsonPropertyName("hw3")]
    public string HW3 { get; init; } = string.Empty;

    [JsonPropertyName("firmware")]
    public string Firmware { get; init; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; init; } = string.Empty;

    /// <summary>
    /// Current usage in Wh
    /// </summary>
    [JsonPropertyName("usage")]
    public double Usage { get; init; }

    /// <summary>
    /// Unit used for the usage
    /// </summary>
    public string Unit { get; } = "Wh";
}
