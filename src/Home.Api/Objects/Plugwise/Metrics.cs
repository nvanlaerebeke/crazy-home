using System.Text.Json.Serialization;

namespace Home.Api.Objects.Plugwise;

public class Metrics {
    [JsonPropertyName("plugs")] 
    public required List<PlugMetric> Plugs { get; init; }
    
}
