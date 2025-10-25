namespace Home.Config;

public interface ISettings {
    IPlugwise Plugwise { get; }
    IMqtt Mqtt { get; }
}
