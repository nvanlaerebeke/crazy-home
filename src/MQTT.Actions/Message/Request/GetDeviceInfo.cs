namespace MQTT.Actions.Message.Request;

internal sealed class GetDeviceInfo : IMqttRequest{
    private readonly string _id;

    public GetDeviceInfo(string id) {
        _id = id;
    }
    public string GetTopic() => $"zigbee2mqtt/{_id}/get";

    public string GetPayload() => string.Empty;
}

