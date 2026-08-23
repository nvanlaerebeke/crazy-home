using Home.CommandLine.Commands;
using Home.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MQTT.Actions;
using Plugwise.Actions;

namespace Home.CommandLine;

public static class Startup {
    public static IServiceCollection AddSettings(this IServiceCollection services) {
        var settings = new SettingsProvider().Get();
        services.AddSingleton(settings);

        services.AddMqtt(settings);
        services.AddPlugwise(new SettingsProvider().Get());

        return services;
    }

    public static IServiceCollection AddCommandLine(this IServiceCollection services) {
        //Configure services/commands
        services.AddTransient<ICommandLineEntryPoint, CommandLineEntryPoint>();
        services.AddTransient<RootCommandEntryPoint>();
        services.AddTransient<Test>();

        //Dependencies
        Test.ConfigureServices(services);

        return services;
    }
}
