using Home.Db;
using LanguageExt.Common;
using MQTT.Actions.Objects;

namespace MQTT.Actions;

public interface IMqttPlugActions {
    Task<Result<List<PlugStatusDto>>> GetAllAsync();
    Task<Result<bool>> SetStateAsync(string id, SwitchState switchState);
    Task<Result<PlugStatusDto?>> GetPlugInfoAsync(string identifier);
    Task<Result<bool>> SetPowerOnBehavior(string id, SwitchState switchState);
    Task<Result<bool>> SetAllowStateChange(string id, bool allowStateChange);
}
