using Home.Config;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Formatter;

namespace MQTT.Actions;

/// <summary>
/// The Mqtt client that connects to the Zigbee controller
/// </summary>
public sealed class MqttClient {
    private readonly ISettings _settings;
    private readonly ILogger<MqttClient> _logger;

    /// <summary>
    /// Lock that makes sure only 1 connect and disconnect happens at a time
    /// </summary>
    private readonly SemaphoreSlim _lock = new(1, 1);

    /// <summary>
    /// List of functions called when a message is received
    /// </summary>
    private readonly List<Func<MqttApplicationMessageReceivedEventArgs, Task>> _onMessageReceivedActions = [];

    /// <summary>
    /// List of functions called when the client connects
    /// </summary>
    private readonly List<Func<MqttClientConnectedEventArgs, Task>> _onConnectedActions = [];

    /// <summary>
    /// List of functions called when the clients go to connecting state
    /// </summary>
    private readonly List<Func<MqttClientConnectingEventArgs, Task>> _onConnectingActions = [];

    /// <summary>
    /// List of functions called when the client disconnects
    /// </summary>
    private readonly List<Func<MqttClientDisconnectedEventArgs, Task>> _onDisconnectedActions = [];

    /// <summary>
    /// Internal Mqtt client
    /// </summary>
    private IMqttClient? _client;

    public MqttClient(ISettings settings, ILogger<MqttClient> logger) {
        _settings = settings;
        _logger = logger;
    }

    /// <summary>
    /// Connect to the MQTT service
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task ConnectAsync() {
        try {
            await _lock.WaitAsync().ConfigureAwait(false);

            if (_client != null) {
                _logger.LogWarning("Already connected");
                return;
            }

            var factory = new MqttClientFactory();
            _client = factory.CreateMqttClient();

            _client.ConnectedAsync += OnConnectedAsync;
            _client.ConnectingAsync += OnConnectingAsync;
            _client.DisconnectedAsync += OnDisconnectedAsync;
            _client.ApplicationMessageReceivedAsync += OnMessageReceivedAsync;

            await _client.ConnectAsync(new MqttClientOptionsBuilder()
                .WithClientId(_settings.Mqtt.ClientName)
                .WithProtocolVersion(MqttProtocolVersion.V500)
                .WithTcpServer(_settings.Mqtt.Broker, _settings.Mqtt.Port)
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(30))
                .WithCleanSession()
                .Build()
            );
            await _client.SubscribeAsync("zigbee2mqtt/#");
        }
        finally {
            _lock.Release();
        }
    }

    /// <summary>
    /// Method called when the client goes into the connecting state
    /// </summary>
    /// <param name="arg"></param>
    private async Task OnConnectingAsync(MqttClientConnectingEventArgs arg) {
        foreach (var action in _onConnectingActions) {
            await action(arg);
        }
    }

    /// <summary>
    /// Method called when a new message is added to the queue
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    private async Task OnMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e) {
        foreach (var onMessageReceivedAction in _onMessageReceivedActions) {
            await onMessageReceivedAction(e);
        }
    }

    /// <summary>
    /// Method called when the client disconnects
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    private async Task OnDisconnectedAsync(MqttClientDisconnectedEventArgs e) {
        if (_client is null) {
            _logger.LogWarning("Already Disconnected");
            return;
        }

        _client.ConnectedAsync -= OnConnectedAsync;
        _client.DisconnectedAsync -= OnDisconnectedAsync;
        _client.ApplicationMessageReceivedAsync -= OnMessageReceivedAsync;

        _onConnectedActions.Clear();
        _onConnectingActions.Clear();
        _onDisconnectedActions.Clear();
        _onMessageReceivedActions.Clear();

        if (e.Exception is not null) {
            _logger.LogError(e.Exception, "Disconnected: {Reason}", e.Reason);
        } else {
            _logger.LogInformation("Disconnected: {Reason}", e.Reason);
        }
        
        foreach (var action in _onDisconnectedActions) {
            await action(e);
        }
    }

    /// <summary>
    /// Method called when the client goes into the connecting state
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    private async Task OnConnectedAsync(MqttClientConnectedEventArgs e) {
        _logger.LogInformation("Connected: {Result}", e.ConnectResult);
        
        foreach (var action in _onConnectedActions) {
            await action(e);
        }
    }

    /// <summary>
    /// Cleanly disconnects the client
    /// </summary>
    public async Task DisconnectAsync() {
        try {
            await _lock.WaitAsync().ConfigureAwait(false);
            if (_client is null) {
                return;
            }

            await _client
                .DisconnectAsync(
                    new MqttClientDisconnectOptionsBuilder()
                        .WithReason(MqttClientDisconnectOptionsReason.NormalDisconnection).Build()
                );
        }
        finally {
            _lock.Release();
        }
    }

    /// <summary>
    /// Adds a function to be called when the client connects
    /// </summary>
    /// <param name="action"></param>
    public void OnConnected(Func<MqttClientConnectedEventArgs, Task> action) {
        _onConnectedActions.Add(action);
    }

    /// <summary>
    /// Add a function to be called when the client disconnects
    /// </summary>
    /// <param name="action"></param>
    public void OnDisconnected(Func<MqttClientDisconnectedEventArgs, Task> action) {
        _onDisconnectedActions.Add(action);
    }

    /// <summary>
    /// Adds a function to be called when the client receives a message
    /// </summary>
    /// <param name="action"></param>
    public void OnMessageReceived(Func<MqttApplicationMessageReceivedEventArgs, Task> action) {
        _onMessageReceivedActions.Add(action);
    }

    /// <summary>
    /// Add a function to be called when the client goes to the connecting state
    /// </summary>
    /// <param name="action"></param>
    public void OnConnecting(Func<MqttClientConnectingEventArgs, Task> action) {
        _onConnectingActions.Add(action);
    }

    public bool IsConnected() => _client is not null;
}
