using MQTT.Actions.Objects;

namespace Home.Api.Objects.Mqtt.ExtensionMethods;

internal static class ToApiObjectExtensionMethods {
    public static PlugStatus ToApiObject(this PlugStatusDto plugStatus) {
        return new() {
            Identifier = plugStatus.Identifier,
            Name = plugStatus.Identifier,
            SwitchState = plugStatus.SwitchState,
            Usage = plugStatus.Usage,
            Current = plugStatus.Current,
            Voltage = plugStatus.Voltage,
            PowerFactor = plugStatus.PowerFactor,
            Unit = "W"
        };
    }

    public static SensorStatus ToApiObject(this SensorDto sensorDto) {
        return new() {
            Id = sensorDto.Id,
            Battery = sensorDto.Battery,
            Humidity = sensorDto.Humidity,
            Temperature = sensorDto.Temperature,
            LinkQuality = sensorDto.LinkQuality
        };
    }
}
