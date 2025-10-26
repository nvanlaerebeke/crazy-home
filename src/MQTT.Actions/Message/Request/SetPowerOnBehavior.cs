using System.Text.Json;
using Home.Db;

namespace MQTT.Actions.Message.Request;

internal sealed class SetPowerOnBehavior : IMqttRequest {
    private readonly string _id;
    private readonly SwitchState _switchState;

    public SetPowerOnBehavior(string id, SwitchState switchState) {
        _id = id;
        _switchState = switchState;
    }

    public override string ToString() {
        var switchState = _switchState == SwitchState.On;
        var payload = JsonSerializer.Serialize(new { power_on_behavior = switchState ? "on" : "off" });
        return payload;
    }

    public string GetTopic() => $"zigbee2mqtt/{_id}/set";

    public string GetPayload() => ToString();
}
