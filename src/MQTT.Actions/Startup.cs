using Microsoft.Extensions.DependencyInjection;
using MQTT.Actions.Cache;
using MQTT.Actions.Services;

namespace MQTT.Actions;

public static class Startup {
    public static IServiceCollection AddMqtt(this IServiceCollection services) {
        services.AddMemoryCache();
        
        services.AddHostedService<EventListener>();
        services.AddHostedService<PlugService>();
        
        services.AddSingleton<MqttClient>();
        services.AddSingleton<PlugCache>();
        return services;
    }
}
