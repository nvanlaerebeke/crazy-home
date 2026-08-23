using Home.Config;
using Microsoft.Extensions.DependencyInjection;
using MQTT.Actions.Actions;
using MQTT.Actions.Cache;
using MQTT.Actions.Message;
using MQTT.Actions.Message.Handlers;
using MQTT.Actions.Services;

namespace MQTT.Actions;

public static class Startup {
    public static IServiceCollection AddMqtt(this IServiceCollection services, ISettings settings) {
        //Caching support, these are singletons
        services.AddMemoryCache();
        services.AddSingleton<BridgeCache>();
        services.AddSingleton<DeviceCache>();
        services.AddSingleton<PlugCache>();
        services.AddSingleton<SensorCache>();
        services.AddSingleton<SwitchCache>();

        //Background Services
        services.AddHostedService<AutoRefreshService>();
        services.AddHostedService<EventListenerService>();
        services.AddHostedService<ReconnectService>();

        //Routing
        services.AddTransient<BridgeInfoMessage>();
        services.AddTransient<DeviceMessage>();
        services.AddTransient<LogMessage>();
        services.AddTransient<MessageRouter>();
        services.AddTransient<PlugMessage>();
        services.AddTransient<SensorMessage>();
        services.AddTransient<SwitchMessage>();

        //Actions
        services.AddTransient<IMqttPlugActions, MqttPlugActions>();
        services.AddTransient<IMqttSensorActions, MqttSensorActions>();
        services.AddTransient<IMqttDeviceActions, MqttDeviceActions>();
        services.AddTransient<IMqttSwitchActions, MqttSwitchActions>();
        services.AddTransient<IMqttPowerActions, MqttPowerActions>();

        services.AddTransient<PermitJoining>();
        
        services.AddTransient<Actions.Plug.GetAll>();
        services.AddTransient<Actions.Plug.GetPlugStatus>();
        services.AddTransient<SetAllowChangeState>();
        services.AddTransient<SetPowerOnBehavior>();
        services.AddTransient<Actions.Plug.SetState>();

        services.AddTransient<Actions.Sensor.GetAll>();
        services.AddTransient<Actions.Sensor.GetSensorStatus>();

        services.AddTransient<Actions.Switch.GetAll>();
        services.AddTransient<Actions.Switch.GetSwitchStatus>();
        services.AddTransient<Actions.Switch.SetState>();
        
        //Misc
        services.AddSingleton<MqttClient>();

        return services;
    }
}
