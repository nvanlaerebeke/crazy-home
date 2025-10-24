using Home.CommandLine.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Home.CommandLine;

public static class Startup {
    public static IServiceCollection AddSettings(this IServiceCollection services) {
        return services;
    }

    public static IServiceCollection AddCommandLine(this IServiceCollection services) {
        var logger = services.BuildServiceProvider().GetRequiredService<ILogger<object>>();

        //Configure services/commands
        services.AddSingleton<ICommandLineEntryPoint, CommandLineEntryPoint>();
        services.AddSingleton<RootCommandEntryPoint>();
        services.AddTransient<Test>();

        //Dependencies
        Test.ConfigureServices(services);

        return services;
    }
}
