namespace MQTT.Actions.Message.Receive.Sensor;

using System.Text.Json.Serialization;

internal sealed class Sensor {
    [JsonPropertyName("battery")]
    public int? Battery { get; set; }

    [JsonPropertyName("humidity")]
    public double? Humidity { get; set; }

    [JsonPropertyName("linkquality")]
    public int? LinkQuality { get; set; }

    [JsonPropertyName("temperature")]
    public double? Temperature { get; set; }

    [JsonPropertyName("humidity_calibration")]
    public double? HumidityCalibration { get; set; }

    [JsonPropertyName("temperature_calibration")]
    public double? TemperatureCalibration { get; set; }

    [JsonPropertyName("update")]
    public UpdateInfo? Update { get; set; }
}
