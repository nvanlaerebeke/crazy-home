namespace Home.Config;

internal class Settings : ISettings {
    public string ConfigDirectory => "/etc/crazy-home";
    public IPlugwise Plugwise => new Plugwise();
    public IMqtt Mqtt => new Mqtt();
    public Auth Auth => new();
    public Audio Audio => new();
}
