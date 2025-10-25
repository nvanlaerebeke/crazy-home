using System.Text.Json;
using System.Text.Json.Serialization;

namespace MQTT.Actions.Message.Request;

internal sealed class PermitJoining : IMqttRequest {
    private readonly TimeSpan _duration;

    [JsonPropertyName("time")]
    public double Time => _duration.TotalSeconds;

    public PermitJoining(TimeSpan duration) {
        _duration = duration;
    }
    public string GetTopic() => "zigbee2mqtt/bridge/request/permit_join";

    public string GetPayload() {
        return JsonSerializer.Serialize(this);
    }
}

