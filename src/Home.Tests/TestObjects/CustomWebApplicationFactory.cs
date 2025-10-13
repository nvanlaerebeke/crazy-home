using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace Home.Tests.TestObjects;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program> {
    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        builder.UseEnvironment("Testing");

        builder.ConfigureTestServices(services => {
            // Example: remove/replace services for testing
            // var descriptor = services.SingleOrDefault(
            //     d => d.ServiceType == typeof(IMyBackgroundService));
            // if (descriptor is not null) services.Remove(descriptor);
            //
            // services.AddSingleton<IMyDependency, FakeMyDependency>();
        });
    }
}
