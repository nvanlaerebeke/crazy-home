namespace Home.Api.Objects.Plugwise;

public class Metrics {
    public Metrics(List<PlugMetric> metrics) {
        Plugs = metrics;
    }

    public List<PlugMetric> Plugs { get; }
}
