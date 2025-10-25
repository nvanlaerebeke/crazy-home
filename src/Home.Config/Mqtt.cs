namespace Home.Config;

internal sealed class Mqtt : IMqtt {
    public string Broker => "localhost"; // or the host/IP where Mosquitto is exposed
    public int Port => 1883; // 8883 if you enabled TLS

    public List<Plug> Plugs => new() {
        new() {
            Identifier = "0xa4c13801e816ffff",
            SourceType = SourceType.Zigbee,
            Name = "Plug1",
            PowerControl = true,
            PowerUsage = true
        },
    };

    public List<string> PlugModelIds => ["S60ZBTPF"]; 
    public List<string> SensorModelIds => ["SNZB-02P"];
    public string ClientName => "home-controller";
}
