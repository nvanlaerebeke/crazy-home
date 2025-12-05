namespace Home.Config;

public interface IMqtt {
    string Broker { get; }
    int Port { get; }
    string ClientName { get; }
    List<string> PlugModelIds { get; }
    List<string> SensorModelIds { get; }
}
