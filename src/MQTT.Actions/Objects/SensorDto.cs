namespace MQTT.Actions.Objects;

public sealed class SensorDto {
    public required string Id { get; init; }
    public required int Battery { get; init; }
    public required double Humidity { get; init; }
    public required double Temperature { get; init; }
    public required int LinkQuality { get; init; }
}

