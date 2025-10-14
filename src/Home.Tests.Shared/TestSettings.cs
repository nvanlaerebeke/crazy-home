using Home.Config;

namespace Home.Tests.Shared;

public sealed class TestSettings : ISettings {
    public List<Plug> Plugs => [
        new("000D6F00004BF588", "Plug1", false, true),
        new("000D6F00004BA1C6", "Plug2", true, true),
    ];
    public string SerialPort => "/dev/USB001";
    public bool PlugwiseBackgroundCaching => false;
}

