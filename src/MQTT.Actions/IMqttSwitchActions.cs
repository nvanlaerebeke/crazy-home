using Home.Db;
using LanguageExt.Common;
using MQTT.Actions.Objects;

namespace MQTT.Actions;

public interface IMqttSwitchActions {
    Task<Result<List<SwitchStatusDto>>> GetAllAsync();
    Task<Result<bool>> SetStateAsync(string id, SwitchState switchState);
    Task<Result<SwitchStatusDto?>> GetSwitchInfoAsync(string identifier);
}
