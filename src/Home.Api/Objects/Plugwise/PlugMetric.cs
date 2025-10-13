using System.Text.Json.Serialization;
using Home.Config;

namespace Home.Api.Objects.Plugwise;

public class PlugMetric {
    public PlugMetric(Plug plug, Usage usage) {
        Mac = plug.Mac;
        Name = plug.Name;
        AllowStateUpdate = plug.PowerControl;
        Usage = usage.PowerConsumption;
        Unit = usage.Unit;
    }

    [JsonPropertyName("allowStateUpdate")]
    public bool AllowStateUpdate { get; }

    [JsonPropertyName("mac")] 
    public string Mac { get; }
    
    [JsonPropertyName("name")] 
    public string Name { get; }

    [JsonPropertyName("usage")] 
    public double Usage { get; }

    [JsonPropertyName("unit")] 
    public string Unit { get; }
}
