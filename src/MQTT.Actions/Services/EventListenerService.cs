using Microsoft.Extensions.Hosting;
using MQTT.Actions.Message;
using MQTTnet;

namespace MQTT.Actions.Services;

/// <summary>
/// Service that listens for incomming messages and routes them to the correctd handler
/// </summary>
internal sealed class EventListenerService : BackgroundService {
    private readonly MqttClient _client;
    private readonly MessageRouter _messageRouter;

    public EventListenerService(
        MqttClient client, 
        MessageRouter messageRouter
    ) {
        _client = client;
        _messageRouter = messageRouter;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        _client.OnMessageReceived(HandleMessageAsync);
        
        if (!_client.IsConnected()) {
            await _client.ConnectAsync();
        }
    }
    
    private async Task HandleMessageAsync(MqttApplicationMessageReceivedEventArgs e) {
        await _messageRouter.RouteAsync(e);
    }
}
