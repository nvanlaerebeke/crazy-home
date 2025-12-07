using Microsoft.Extensions.Logging;

namespace MQTT.Actions.Message.Handlers;

internal sealed class LogMessage: IMessageHandler  {
    private readonly ILogger<LogMessage> _logger;

    public LogMessage(ILogger<LogMessage> logger) {
        _logger = logger;
    }
    
    public Task HandleAsync(string topic, string payload) {
        _logger.Log(LogLevel.Information, "[Log] {PayLoad}", payload);
        return Task.CompletedTask;
    }

    public bool AcceptsTopic(string topic) {
        return topic.Equals("zigbee2mqtt/bridge/logging");
    }
}

