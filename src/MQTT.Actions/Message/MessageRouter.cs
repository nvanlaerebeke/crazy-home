using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MQTT.Actions.Message.Handlers;
using MQTTnet;

namespace MQTT.Actions.Message;

internal sealed class MessageRouter {
    private readonly List<IMessageHandler> _routers = [];
    private readonly ILogger<MessageRouter> _logger;

    public MessageRouter(IServiceProvider serviceProvider, ILogger<MessageRouter> logger) {
        _logger = logger;
        
        _routers.Add(serviceProvider.GetRequiredService<BridgeInfoMessage>());
        _routers.Add(serviceProvider.GetRequiredService<DeviceMessage>());
        _routers.Add(serviceProvider.GetRequiredService<LogMessage>());
        _routers.Add(serviceProvider.GetRequiredService<PlugMessage>());
        _routers.Add(serviceProvider.GetRequiredService<SensorMessage>());
        _routers.Add(serviceProvider.GetRequiredService<SwitchMessage>());
    }

    public async Task RouteAsync(MqttApplicationMessageReceivedEventArgs eventArgs) {
        var topic = eventArgs.ApplicationMessage.Topic ?? string.Empty;
        var payload = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);
        
        try {
            //Routing is based on the topic, no topic = unroutable
            if (string.IsNullOrEmpty(topic)) {
                _logger.LogError("Received message, but no topic specified: {PayLoad}", payload);
                return;
            }

            _logger.LogInformation("Received message from {ClientName}", topic);

            var router = _routers.FirstOrDefault(x => x.AcceptsTopic(topic));
            if (router is null) {
                _logger.LogWarning("Could not find router for {Topic}", topic);
                return;
            }

            await router.HandleAsync(topic, payload);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to process message: {Topic} => {Payload}", topic, payload);
        }
    }
}

