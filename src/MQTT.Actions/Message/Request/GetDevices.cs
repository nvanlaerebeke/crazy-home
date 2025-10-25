namespace MQTT.Actions.Message.Request;

internal sealed class GetDevices : IMqttRequest {
    public string GetTopic() => "zigbee2mqtt/bridge/request/device/get";

    public string GetPayload() => string.Empty;
}
