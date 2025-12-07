using System.Text;
using System.Text.Json;
using Home.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MQTT.Actions.Cache;
using MQTT.Actions.Message;
using MQTT.Actions.Objects.ExtensionMethods;
using MQTTnet;

namespace MQTT.Actions.Actions;

internal sealed class SetPowerOnBehavior {
    private readonly MqttClient _client;
    private readonly DeviceCache _deviceCache;
    private readonly PlugCache _plugCache;
    private readonly HomeDbContextFactory _dbContextFactory;
    private readonly ILogger<SetPowerOnBehavior> _logger;

    private CancellationTokenSource? _cancellationTokenSource;
    private string? _identifier;
    private SwitchState? _switchState;
    
    public SetPowerOnBehavior(
        MqttClient client,
        DeviceCache deviceCache,
        PlugCache plugCache, 
        HomeDbContextFactory dbContextFactory, 
        ILogger<SetPowerOnBehavior> logger
    ) {
        _client = client;
        _deviceCache = deviceCache;
        _plugCache = plugCache;
        _dbContextFactory = dbContextFactory;
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
            await _client.SendAsync(new Message.Request.SetPowerOnBehavior(identifier, switchState));

            //Wait a maximum of 10 seconds or a confirmation that the state was changed
            _cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(10));
            await using var _ = _cancellationTokenSource.Token.Register(() => tcs.SetResult());

            //Wait until completion or time-out
            await tcs.Task;

            //Check if the status was set correctly
            return _plugCache.Get(identifier)?.PowerOnBehavior == switchState;
        } catch (OperationCanceledException) {
            _logger.LogError("OperationCanceledException");
        } finally {
            //Stop listening for messages
            _client.UnSubscribeOnMessageReceived(CheckStateChanged);
        }
        return false;
    }

    /// <summary>
    /// ToDo: figure out a way to reliably get the 'updated' state of the plug
    ///       Best after the "set", to call a "get" manually so the property can be checked
    ///       Not all devices & firmware's send an updated status by themselves 
    /// </summary>
    /// <param name="eventArgs"></param>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task CheckStateChanged(MqttApplicationMessageReceivedEventArgs eventArgs) {
        var payload = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);

        //Check
        if (_identifier is null || _switchState is null || _cancellationTokenSource is null) {
            throw new InvalidOperationException("Invalid State");
        }

        //Is this for this plug/identifier?
        if (!eventArgs.ApplicationMessage.Topic.EndsWith(_identifier)) {
            return;
        }

        //Check if the message is a PlugStatus payload
        var plugStatus = JsonSerializer.Deserialize<PlugStatus>(payload);
        if (plugStatus is null) {
            return;
        }

        //Plug state was not changed yet
        if (!plugStatus.PowerOnBehavior.Equals(_switchState.ToString(), StringComparison.CurrentCultureIgnoreCase)) {
            return;
        }

        //Plug state was updated to ON, set it in the cache and set the continuation token
        await using var work = await _dbContextFactory.GetAsync();
        var device = await work.Devices.FirstOrDefaultAsync(x => x.IeeeAddress.Equals(_identifier));
        if (device is null) {
            await _cancellationTokenSource.CancelAsync();
            return;
        }

        if (device.PowerOnBehavior != _switchState) {
            device.PowerOnBehavior = _switchState.GetValueOrDefault();
            work.Devices.Update(device);
            await work.SaveChangesAsync();
        }

        _deviceCache.UpdateDevice(device);
        _plugCache.Set(plugStatus.ToDto(device));
        await _cancellationTokenSource.CancelAsync();
    }
}
