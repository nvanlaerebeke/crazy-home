namespace MQTT.Actions.Message.Request;

internal interface IMqttRequest {
    public string GetTopic();
    public string GetPayload();
}
