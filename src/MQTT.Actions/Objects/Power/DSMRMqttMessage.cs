using MQTT.Actions.Message.Request;

namespace MQTT.Actions.Objects.Power;

internal sealed class DsmrMqttMessage : IMqttRequest {
    private readonly string _topic;
    private readonly string _payload;

    public DsmrMqttMessage(string topic, string payload) {
        _topic = topic;
        _payload = payload;
    }
    public string GetTopic() => _topic;

    public string GetPayload() => _payload;
}
