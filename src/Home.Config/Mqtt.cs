namespace Home.Config;

internal sealed class Mqtt : IMqtt {
    public string Broker {
        get {
            var envVar = Environment.GetEnvironmentVariable("MQTT_BROKER");
            return string.IsNullOrEmpty(envVar) ? "localhost" : envVar;
        }
    }

    public int Port {
        get {
            var envVar = Environment.GetEnvironmentVariable("MQTT_PORT");
            if (string.IsNullOrEmpty(envVar)) {
                return 1883;
            }

            return int.TryParse(envVar, out var port) ? port : 1883;
        }
    }

    public List<string> PlugModelIds => ["S60ZBTPF"];
    public List<string> SensorModelIds => ["SNZB-02P"];
    public string ClientName => "home-controller";
}
