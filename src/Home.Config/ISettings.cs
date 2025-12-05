namespace Home.Config;

public interface ISettings {
    string ConfigDirectory { get; }
    IPlugwise Plugwise { get; }
    IMqtt Mqtt { get; }
    Auth Auth { get; }
}
