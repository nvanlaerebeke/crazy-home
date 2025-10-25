namespace Home.Config;

public interface IMqtt {
    string Broker { get; }
    int Port { get; }
    List<Plug> Plugs { get; }
    string ClientName { get; }
    List<string> PlugModelIds { get; }
    List<string> SensorModelIds { get; }
}
