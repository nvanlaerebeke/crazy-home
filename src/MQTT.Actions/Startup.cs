using Microsoft.Extensions.DependencyInjection;
using MQTT.Actions.Cache;
using MQTT.Actions.Message;
using MQTT.Actions.Message.Handlers;
using MQTT.Actions.Services;

namespace MQTT.Actions;

public static class Startup {
    public static IServiceCollection AddMqtt(this IServiceCollection services) {
        //Caching support
        services.AddMemoryCache();
        services.AddSingleton<BridgeCache>();
        services.AddSingleton<DeviceCache>();
        services.AddSingleton<PlugCache>();
        services.AddSingleton<SensorCache>();

        //Background Services
        services.AddHostedService<EventListenerService>();
        services.AddHostedService<PlugService>();

        //Routing
        services.AddSingleton<BridgeInfoMessage>();
        services.AddSingleton<DeviceMessage>();
        services.AddSingleton<LogMessage>();
        services.AddSingleton<MessageRouter>();
        services.AddSingleton<PlugMessage>();
        services.AddSingleton<SensorMessage>();

        //Actions
        services.AddSingleton<IMqttPlugActions, MqttPlugActions>();
        services.AddSingleton<IMqttSensorActions, MqttSensorActions>();

        services.AddTransient<Actions.Plug.GetAll>();
        services.AddTransient<Actions.Plug.GetPlugStatus>();
        services.AddTransient<Actions.Plug.SetState>();

        services.AddTransient<Actions.Sensor.GetAll>();
        services.AddTransient<Actions.Sensor.GetSensorStatus>();

        //Misc
        services.AddSingleton<MqttClient>();

        return services;
    }
}
