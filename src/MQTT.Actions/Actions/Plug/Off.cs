namespace MQTT.Actions.Actions.Plug;

internal sealed class Off {
    public Task<bool> ExecuteAsync() {
        return Task.FromResult(true);
    }
}

