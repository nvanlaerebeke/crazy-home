namespace MQTT.Actions.Actions;

internal sealed class PermitJoining {
    private readonly MqttClient _client;

    public PermitJoining(MqttClient client) {
        _client = client;
    }
    public async Task<bool> ExecuteAsync() {
        await _client.SendAsync(new Message.Request.PermitJoining(TimeSpan.FromSeconds(120)));
        return true;
    }
}

