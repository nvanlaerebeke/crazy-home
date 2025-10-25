using Home.Api;
using Microsoft.AspNetCore.Builder;

namespace Home.Tests.TestObjects;

internal class TestSetup : Startup {
    public void Start(WebApplicationBuilder builder) {
        base.Start(builder);
    }

    protected override void ConfigureApp(WebApplication app) {
        base.ConfigureApp(app);
    }
}
