using Home.CommandLine.Commands;
using Home.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MQTT.Actions;
using Plugwise.Actions;

namespace Home.CommandLine;

public static class Startup {
    public static IServiceCollection AddSettings(this IServiceCollection services) {
        services.AddSingleton(new SettingsProvider().Get());
        
        services.AddMqtt();
        services.AddPlugwise(new SettingsProvider().Get());

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
