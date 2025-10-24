namespace Home.Config;

public class Plug {
    public string Mac { get; }
    public string Name { get; }
    public bool PowerControl { get; }
    public bool PowerUsage { get; }

    public Plug(string mac, string name, bool powerControl, bool powerUsage) {
        Mac = mac;
        Name = name;
        PowerControl = powerControl;
        PowerUsage = powerUsage;
    }
}
