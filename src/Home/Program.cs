using Home.CommandLine;
using Startup = Home.Api.Startup;

namespace Home;

public partial class Program {
    private static Task Main(string[] args) {
        if (args.Length > 0) {
            var host = Host
                .CreateDefaultBuilder()
                .ConfigureServices((_, services) => { services.AddSettings().AddCommandLine(); })
                .Build();

            return host.Services.GetRequiredService<ICommandLineEntryPoint>().StartAsync(args);
        }

        new Startup().Start(WebApplication.CreateBuilder(args));
        return Task.CompletedTask;
    }
}
