namespace Home.Config;

internal class Settings : ISettings {
    public IPlugwise Plugwise => new Plugwise();
    public IMqtt Mqtt => new Mqtt();
}
