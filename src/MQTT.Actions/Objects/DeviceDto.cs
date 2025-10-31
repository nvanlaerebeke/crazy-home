using Home.Db;

namespace MQTT.Actions.Objects;

public sealed class DeviceDto {
    public required DeviceType Type { get; init; }
    public required string IeeeAddress { get; init; }
    public required string FriendlyName { get; init; }
}

