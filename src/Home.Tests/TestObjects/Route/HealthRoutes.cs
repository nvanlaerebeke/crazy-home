namespace Home.Tests.TestObjects.Route;

internal sealed class HealthRoutes {
    public string Live() => "/livez";
    public string Ready() => "/readyz";
    public string Health() => "/healthz";
}

