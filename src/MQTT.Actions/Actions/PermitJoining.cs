namespace MQTT.Actions.Actions;

internal sealed class PermitJoining {
    private static readonly TimeSpan JoinDuration = TimeSpan.FromSeconds(120);
    private readonly MqttClient _client;

    public PermitJoining(MqttClient client) {
        _client = client;
    }

    public async Task<double> ExecuteAsync(TimeSpan? duration = null) {
        await _client.SendAsync(new Message.Request.PermitJoining(duration ?? JoinDuration));
        return (duration ?? JoinDuration).TotalSeconds;
    }
}
