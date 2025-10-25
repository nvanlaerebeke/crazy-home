namespace Home.Config;

public interface IPlugwise {
    bool BackgroundCaching { get; }
    List<Plug> Plugs { get; }
    string SerialPort { get; }
}
