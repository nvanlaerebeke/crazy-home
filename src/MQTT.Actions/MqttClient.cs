using System.Text.Json;
using MQTTnet;
using MQTTnet.Formatter;

namespace MQTT.Actions;

public sealed class MqttClient {
    public async Task Connect() {
        var factory = new MqttClientFactory();
        var client = factory.CreateMqttClient();

        var options = new MqttClientOptionsBuilder()
            .WithClientId("nico-dev-1")
            .WithProtocolVersion(MqttProtocolVersion.V311)
            .WithTcpServer("tcp://mqtt.crazyzone.be", 6638) // use 8883 + TLS if required
            //.WithCredentials("user", "pass")
            //.WithTlsOptions(o => o.UseTls())            // uncomment if using 8883
            .WithKeepAlivePeriod(TimeSpan.FromSeconds(30))
            .WithCleanSession()
            .Build();

        client.ConnectedAsync += e => {
            Console.WriteLine($"Connected: {e.ConnectResult}");
            return Task.CompletedTask;
        };
        client.DisconnectedAsync += e => {
            Console.WriteLine($"Disconnected: {e.Reason}");
            if (e.Exception != null) {
                Console.WriteLine(e.Exception);
            }

            return Task.CompletedTask;
        };

        await client.ConnectAsync(options);

        // Example when using Zigbee2MQTT
        await client.SubscribeAsync("zigbee2mqtt/#");
        await Task.Delay(Timeout.Infinite); // keep running
    }

    public static async Task Clean_Disconnect() {
        /*
         * This sample disconnects in a clean way. This will send a MQTT DISCONNECT packet
         * to the server and close the connection afterward.
         *
         * See sample _Connect_Client_ for more details.
         */

        var mqttFactory = new MqttClientFactory();

        using var mqttClient = mqttFactory.CreateMqttClient();
        var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("broker.hivemq.com").Build();
        await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

        // This will send the DISCONNECT packet. Calling _Dispose_ without DisconnectAsync the
        // connection is closed in a "not clean" way. See MQTT specification for more details.
        await mqttClient.DisconnectAsync(new MqttClientDisconnectOptionsBuilder()
            .WithReason(MqttClientDisconnectOptionsReason.NormalDisconnection).Build());
    }
}

internal static class ObjectExtensions {
    public static TObject DumpToConsole<TObject>(this TObject @object) {
        var output = "NULL";
        if (@object != null) {
            output = JsonSerializer.Serialize(@object, new JsonSerializerOptions { WriteIndented = true });
        }

        Console.WriteLine($"[{@object?.GetType().Name}]:\r\n{output}");
        return @object;
    }
}
