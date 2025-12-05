using Home.Tests.TestObjects.Route;

namespace Home.Tests.TestObjects;

internal static class Routes {
    public static HealthRoutes Health => new();
    public static MetricRoutes Metrics => new();
    public static PlugRoutes Plugs => new();
}
