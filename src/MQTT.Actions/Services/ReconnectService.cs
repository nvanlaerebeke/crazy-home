using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;

namespace MQTT.Actions.Services;

internal sealed class ReconnectService : BackgroundService {
    private readonly MqttClient _client;
    private readonly ILogger<ReconnectService> _logger;

    public ReconnectService(MqttClient client, ILogger<ReconnectService> logger) {
        _client = client;
        _logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        _client.OnDisconnected(OnDisconnected);
        
        try {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        } catch (Exception ex) {
            _logger.LogError(ex, "Error in client reconnect service");
        } finally {
            _client.UnSubscribeOnDisconnected(OnDisconnected);
        }
    }

    private async Task OnDisconnected(MqttClientDisconnectedEventArgs eventArgs) {
        if (_client.IsConnected()) {
            return;
        }

        while (!_client.IsConnected()) {
            await _client.ConnectAsync();
            await Task.Delay(1000);
            if (!_client.IsConnected()) {
                _logger.LogError("Reconnect failed");
            } else {
                _logger.LogInformation("Reconnect success");
            }
        }
    }
}

