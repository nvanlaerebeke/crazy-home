using LanguageExt.Common;
using MQTT.Actions.Objects;

namespace MQTT.Actions;

public interface IMqttSensorActions {
    Task<Result<List<SensorDto>>> GetAllAsync();
    Task<Result<SensorDto?>> GetSensorStatusAsync(string id);
    Task<Result<double>> PermitJoiningAsync();
}
