using System.Text.Json;
using Home.Shared;

namespace MQTT.Actions.Message.Request;

internal sealed class SetState : IMqttRequest {
    private readonly string _id;
    private readonly SwitchState _switchState;

    public SetState(string id, SwitchState switchState) {
        _id = id;
        _switchState = switchState;
    }

    public override string ToString() {
        var switchState = _switchState == SwitchState.On;
        var payload = JsonSerializer.Serialize(new { state = switchState ? "ON" : "OFF" });
        return payload;
    }

    public string GetTopic() => $"zigbee2mqtt/{_id}/set";

    public string GetPayload() => ToString();
}
