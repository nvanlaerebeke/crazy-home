namespace Home.Config;

internal sealed class Plugwise : IPlugwise {
    public bool BackgroundCaching => true;

    public List<Plug> Plugs => new() {
        new() {
            Identifier = "000D6F0001A5A3B6",
            SourceType = SourceType.Plugwise,
            Name = "Rack",
            PowerControl = false,
            PowerUsage = true
        },
        new() {
            Identifier = "000D6F00004BF588",
            SourceType = SourceType.Plugwise,
            Name = "Desk Nico",
            PowerControl = false,
            PowerUsage = true
        },
        new() {
            Identifier = "000D6F00004BA1C6",
            SourceType = SourceType.Plugwise,
            Name = "Lego Display",
            PowerControl = true,
            PowerUsage = true
        },
        new() {
            Identifier = "000D6F00004B992C",
            SourceType = SourceType.Plugwise,
            Name = "Purple Christmas Tree",
            PowerControl = true,
            PowerUsage = true
        },
        new() {
            Identifier = "000D6F0000D31AB8",
            SourceType = SourceType.Plugwise,
            Name = "Disney Christmas Tree",
            PowerControl = true,
            PowerUsage = true
        },
        new() {
            Identifier = "000D6F00004B9EA7",
            SourceType = SourceType.Plugwise,
            Name = "Kitchen",
            PowerControl = true,
            PowerUsage = true
        },
        new() {
            Identifier = "000D6F00004BC20A",
            SourceType = SourceType.Plugwise,
            Name = "Book Case",
            PowerControl = true,
            PowerUsage = true
        },
        new() {
            Identifier = "000D6F000076B9B3",
            SourceType = SourceType.Plugwise,
            Name = "Bar Cabinet",
            PowerControl = true,
            PowerUsage = true
        },
        /*new() {
            Identifier = "000D6F00004BA287",
            SourceType = SourceType.Plugwise,
            Name = "Aquarium",
            PowerControl = false,
            PowerUsage = true
        },*/
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
}
