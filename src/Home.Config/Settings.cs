namespace Home.Config;

internal class Settings : ISettings {
    public List<Plug> Plugs => new() {
        new("000D6F0001A5A3B6", "Rack", false, true),
        new("000D6F00004BF588", "Desk Nico", false, true),
        new("000D6F00004BA1C6", "Lego Display", true, true),
        new("000D6F00004B992C", "Purple Christmas Tree", true, true),
        new("000D6F0000D31AB8", "Disney Christmas Tree", true, true),
        new("000D6F00004B9EA7", "Kitchen", true, true),
        new("000D6F00004BC20A", "Book Case", true, true),
        new("000D6F000076B9B3", "Bar Cabinet", true, true),
        //new("000D6F00004BA287", "Aquarium", true, true) 
    };
    
    public string SerialPort {
        get {
            var serialPort = Environment.GetEnvironmentVariable("PLUGWISE_SERIAL_PORT");
            if (serialPort is null) {
                return string.Empty;
            }

            return !File.Exists(serialPort) ? string.Empty : serialPort;
        }
    }

    public bool PlugwiseBackgroundCaching => true;
}
