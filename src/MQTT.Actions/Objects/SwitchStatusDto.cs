using Home.Db;

namespace MQTT.Actions.Objects;

public sealed class SwitchStatusDto {
    public required string Id { get; init; }
    public required string Name { get; init; }

    public required SwitchState SwitchState { get; init; }
}
