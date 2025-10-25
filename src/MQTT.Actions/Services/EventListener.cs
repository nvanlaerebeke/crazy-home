using System.Text;
using System.Text.Json;
using Home.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTT.Actions.Cache;
using MQTT.Actions.Message;
using MQTTnet;

namespace MQTT.Actions.Services;

internal sealed class EventListener : BackgroundService {
    private readonly MqttClient _client;
    private readonly ISettings _settings;
    private readonly PlugCache _plugCache;
    private readonly ILogger<EventListener> _logger;

    public EventListener(
        MqttClient client, 
        ISettings settings,
        PlugCache plugCache,
        ILogger<EventListener> logger
    ) {
        _client = client;
        _settings = settings;
        _plugCache = plugCache;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        _client.OnMessageReceived(HandleMessageAsync);
        
        if (!_client.IsConnected()) {
            await _client.ConnectAsync();
        }
    }
    
    private async Task HandleMessageAsync(MqttApplicationMessageReceivedEventArgs e) {
        _logger.LogInformation("Received message from {ClientName}", e.ApplicationMessage.Topic);
        var plug = _settings.Mqtt.Plugs.FirstOrDefault(x => e.ApplicationMessage.Topic.EndsWith(x.Identifier));
        if (plug is null) {
            _logger.LogWarning("Could not find device for {Identifier}", e.ApplicationMessage.Topic);
            return;
        }
        
        var topic = e.ApplicationMessage.Topic ?? "";
        var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
        _logger.LogInformation("[Zigbee][{Topic}] {Payload}", topic, payload);
        var plugStatus = JsonSerializer.Deserialize<PlugStatus>(payload);
        if (plugStatus is null) {
            return;
        }
        
        _plugCache.Set(plug.Identifier, plugStatus);
    }
}
