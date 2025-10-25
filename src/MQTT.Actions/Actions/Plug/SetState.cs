using System.Text;
using System.Text.Json;
using Home.Shared;
using Microsoft.Extensions.Logging;
using MQTT.Actions.Cache;
using MQTT.Actions.Message;
using MQTT.Actions.Objects.ExtensionMethods;
using MQTTnet;

namespace MQTT.Actions.Actions.Plug;

internal sealed class SetState {
    private readonly MqttClient _client;
    private readonly PlugCache _cache;
    private readonly ILogger<SetState> _logger;

    private CancellationTokenSource? _cancellationTokenSource;
    private string? _identifier;
    private SwitchState? _switchState;
    
    public SetState(MqttClient client, PlugCache cache, ILogger<SetState> logger) {
        _client = client;
        _cache = cache;
        _logger = logger;
    }

    public async Task<bool> ExecuteAsync(string identifier, SwitchState switchState) {
        if (_identifier is not null || _cancellationTokenSource is not null) {
            throw new InvalidOperationException("Action was already executed");
        }

        _identifier = identifier;
        _switchState = switchState;
        
        var tcs = new TaskCompletionSource();
        _cancellationTokenSource = new CancellationTokenSource();

        try {
            //Start listening for messages
            _client.OnMessageReceived(CheckStateChanged);

            //Send the request to set the state
            await _client.SendAsync(new Message.Request.SetState(identifier, switchState));

            //Wait a maximum of 10 seconds or a confirmation that the state was changed
            _cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(10));
            await using var _ = _cancellationTokenSource.Token.Register(() => tcs.SetResult());

            //Wait until completion or time-out
            await tcs.Task;

            //Check if the status was set correctly
            return _cache.Get(identifier)?.SwitchState == switchState;
        } catch (OperationCanceledException) {
            _logger.LogError("OperationCanceledException");
        } finally {
            //Stop listening for messages
            _client.UnSubscribeOnMessageReceived(CheckStateChanged);
        }
        return false;
    }

    private Task CheckStateChanged(MqttApplicationMessageReceivedEventArgs eventArgs) {
        //Check
        if (_identifier is null || _switchState is null || _cancellationTokenSource is null) {
            throw new InvalidOperationException("Invalid State");
        }

        //Is this for this plug/identifier?
        if (!eventArgs.ApplicationMessage.Topic.EndsWith(_identifier)) {
            return Task.CompletedTask;
        }

        //Check if the message is a PlugStatus payload
        var payload = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);
        var plugStatus = JsonSerializer.Deserialize<PlugStatus>(payload);
        if (plugStatus is null) {
            return Task.CompletedTask;
        }

        //Plug state was not changed to ON yet
        if (!plugStatus.State.Equals(_switchState.ToString(), StringComparison.CurrentCultureIgnoreCase)) {
            return Task.CompletedTask;
        }

        //Plug state was updated to ON, set it in the cache and set the continuation token
        _cache.Set(plugStatus.ToDto(_identifier));
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }
}
