using MQTT.Actions.Objects;

namespace Home.Api.Objects.Mqtt.ExtensionMethods;

internal static class ToApiObjectExtensionMethods {
    public static PlugStatus ToApiObject(this PlugStatusDto plugStatus) {
        return new() {
            Identifier = plugStatus.Id,
            Name = plugStatus.Name,
            SwitchState = plugStatus.SwitchState,
            Usage = plugStatus.Usage,
            Current = plugStatus.Current,
            Voltage = plugStatus.Voltage,
            PowerFactor = plugStatus.PowerFactor,
            Unit = "W",
            AllowStateChange = plugStatus.AllowStateChange,
            PowerOnBehavior = plugStatus.PowerOnBehavior
        };
    }

    public static SensorStatus ToApiObject(this SensorDto sensorDto) {
        return new() {
            Id = sensorDto.Id,
            Name = sensorDto.Name,
            Battery = sensorDto.Battery,
            Humidity = sensorDto.Humidity,
            Temperature = sensorDto.Temperature,
            LinkQuality = sensorDto.LinkQuality
        };
    }

    public static Device ToApiObject(this DeviceDto device) {
        return new() {
            DeviceType = device.Type,
            IeeeAddress = device.IeeeAddress,
            FriendlyName = device.FriendlyName,
        }; 
    }
}
