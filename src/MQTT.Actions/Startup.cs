using Home.Config;
using Microsoft.Extensions.DependencyInjection;
using MQTT.Actions.Actions;
using MQTT.Actions.Cache;
using MQTT.Actions.Message;
using MQTT.Actions.Message.Handlers;
using MQTT.Actions.Services;
using Home.Db;

namespace MQTT.Actions;

public static class Startup {
    public static IServiceCollection AddMqtt(this IServiceCollection services, ISettings settings) {
        //Configure database
        services.AddDatabase(settings);
        
        //Caching support
        services.AddMemoryCache();
        services.AddSingleton<BridgeCache>();
        services.AddSingleton<DeviceCache>();
        services.AddSingleton<PlugCache>();
        services.AddSingleton<SensorCache>();

        //Background Services
        services.AddHostedService<AutoRefreshService>();
        services.AddHostedService<EventListenerService>();
        services.AddHostedService<PlugService>();
        services.AddHostedService<ReconnectService>();

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
        services.AddSingleton<IMqttDeviceActions, MqttDeviceActions>();

        services.AddSingleton<PermitJoining>();
        
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
