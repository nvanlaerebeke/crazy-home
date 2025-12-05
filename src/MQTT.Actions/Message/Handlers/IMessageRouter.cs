namespace MQTT.Actions.Message.Handlers;

internal interface IMessageRouter {
    Task RouteAsync(string topic, string payload);
    bool AcceptsTopic(string topic);
}
