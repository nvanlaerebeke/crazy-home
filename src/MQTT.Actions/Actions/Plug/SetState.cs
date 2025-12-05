using System.Text;
using System.Text.Json;
using Home.Db;
using Home.Error;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MQTT.Actions.Cache;
using MQTT.Actions.Message;
using MQTT.Actions.Objects.ExtensionMethods;
using MQTTnet;

namespace MQTT.Actions.Actions.Plug;

internal sealed class SetState {
    private readonly MqttClient _client;
    private readonly PlugCache _cache;
    private readonly HomeDbContextFactory _dbContextFactory;
    private readonly ILogger<SetState> _logger;

    private CancellationTokenSource? _cancellationTokenSource;
    private string? _identifier;
    private SwitchState? _switchState;

    public SetState(
        MqttClient client, PlugCache cache, HomeDbContextFactory dbContextFactory, ILogger<SetState> logger
    ) {
        _client = client;
        _cache = cache;
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }

    public async Task<bool> ExecuteAsync(string id, SwitchState switchState) {
        if (_identifier is not null || _cancellationTokenSource is not null) {
            throw new InvalidOperationException("Action was already executed");
        }

        await using (var work = await _dbContextFactory.GetAsync()) {
            var device = work.Devices.FirstOrDefault(d => d.IeeeAddress == id);
            if (device == null) {
                throw HomeApiException.from(ApiErrorCode.NotFound);
            }

            if (!device.AllowStateChange) {
                throw new UnauthorizedAccessException();
            }
        }

        _identifier = id;
        _switchState = switchState;

        var tcs = new TaskCompletionSource();
        _cancellationTokenSource = new CancellationTokenSource();

        try {
            //Start listening for messages
            _client.OnMessageReceived(CheckStateChanged);

            //Send the request to set the state
            await _client.SendAsync(new Message.Request.SetState(id, switchState));

            //Wait a maximum of 10 seconds or a confirmation that the state was changed
            _cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(10));
            await using var _ = _cancellationTokenSource.Token.Register(() => tcs.SetResult());

            //Wait until completion or time-out
            await tcs.Task;

            //Check if the status was set correctly
            return _cache.Get(id)?.SwitchState == switchState;
        } catch (OperationCanceledException) {
            _logger.LogError("OperationCanceledException");
        } finally {
            //Stop listening for messages
            _client.UnSubscribeOnMessageReceived(CheckStateChanged);
        }

        return false;
    }

    private async Task CheckStateChanged(MqttApplicationMessageReceivedEventArgs eventArgs) {
        //Check
        if (_identifier is null || _switchState is null || _cancellationTokenSource is null) {
            throw new InvalidOperationException("Invalid State");
        }

        //Is this for this plug/identifier?
        if (!eventArgs.ApplicationMessage.Topic.EndsWith(_identifier)) {
            return;
        }

        //Check if the message is a PlugStatus payload
        var payload = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);
        var plugStatus = JsonSerializer.Deserialize<PlugStatus>(payload);
        if (plugStatus is null) {
            return;
        }

        //Plug state was not changed to ON yet
        if (!plugStatus.State.Equals(_switchState.ToString(), StringComparison.CurrentCultureIgnoreCase)) {
            return;
        }

        //Plug state was updated to ON, set it in the cache and set the continuation token
        await using var work = await _dbContextFactory.GetAsync();
        var device = await work.Devices.FirstOrDefaultAsync(x => x.IeeeAddress.Equals(_identifier));
        if (device is null) {
            _logger.LogError("Device not found: {Identifier}", _identifier);
            await _cancellationTokenSource.CancelAsync();
            return;
        }

        _cache.Set(plugStatus.ToDto(device));
        await _cancellationTokenSource.CancelAsync();
    }
}
