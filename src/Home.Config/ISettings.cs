namespace Home.Config;

public interface ISettings {
    List<Plug> Plugs { get; }
    string SerialPort { get; }
    bool PlugwiseBackgroundCaching { get; }
}
