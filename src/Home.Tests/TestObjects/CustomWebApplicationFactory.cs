using Home.Config;
using Home.Tests.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Home.Tests.TestObjects;

internal sealed class CustomWebApplicationFactory : WebApplicationFactory<Program> {
    internal ExternalServices ExternalServices { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        builder.UseEnvironment("Testing");

        builder.ConfigureTestServices(services => {
            services.AddSingleton(ExternalServices.RequestManager.Object);
            services.AddSingleton<ISettings>(new TestSettings());
        });
    }
}
