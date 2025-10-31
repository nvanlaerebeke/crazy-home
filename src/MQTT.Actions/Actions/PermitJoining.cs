namespace MQTT.Actions.Actions;

internal sealed class PermitJoining {
    private static readonly TimeSpan JoinDuration = TimeSpan.FromSeconds(120);
    private readonly MqttClient _client;

    public PermitJoining(MqttClient client) {
        _client = client;
    }
    public async Task<double> ExecuteAsync() {
        await _client.SendAsync(new Message.Request.PermitJoining(JoinDuration));
        return JoinDuration.TotalSeconds;
    }
}

