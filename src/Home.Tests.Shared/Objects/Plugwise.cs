using Home.Config;

namespace Home.Tests.Shared.Objects;

internal sealed class Plugwise : IPlugwise {
    public bool BackgroundCaching => false;

    public List<Plug> Plugs => [
        new() {
            Identifier = "000D6F00004BF588",
            SourceType = SourceType.Plugwise,
            Name = "Plug1",
            PowerControl = false,
            PowerUsage = true
        },
        new() {
            Identifier = "000D6F00004BA1C6",
            SourceType = SourceType.Plugwise,
            Name = "Plug2",
            PowerControl = true,
            PowerUsage = true
        }
    ];

    public string SerialPort => "/dev/USB001";
}
