using System.Text.Json.Serialization;

namespace Home.Api.Objects.Plugwise;

public class Usage {
    public Usage(double usage, string unit) {
        PowerConsumption = usage;
        Unit = unit;
    }

    [JsonPropertyName("power_consumption")]
    public double PowerConsumption { get; }

    [JsonPropertyName("unit")] public string Unit { get; }
}
