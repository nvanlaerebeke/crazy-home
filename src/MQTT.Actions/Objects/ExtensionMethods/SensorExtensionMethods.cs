using MQTT.Actions.Message.Receive.Sensor;

namespace MQTT.Actions.Objects.ExtensionMethods;

internal static class SensorExtensionMethods {
    public static SensorDto ToDto(this Sensor sensor, string id) {
        return new() {
            Id = id,
            Battery = sensor.Battery ?? 0,
            Humidity = sensor.Humidity ?? 0,
            Temperature = sensor.Temperature ?? 0,
            LinkQuality = sensor.LinkQuality ?? 0 
        };
    }
}

