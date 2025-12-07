namespace MQTT.Actions.Message.Handlers;

internal interface IMessageHandler {
    Task HandleAsync(string topic, string payload);
    bool AcceptsTopic(string topic);
}
