using Home.Config;
using Home.Tests.Shared.Objects;

namespace Home.Tests.Shared;

public sealed class TestSettings : ISettings {
    public IPlugwise Plugwise => new Plugwise();
    public IMqtt Mqtt => new Mqtt();
}
