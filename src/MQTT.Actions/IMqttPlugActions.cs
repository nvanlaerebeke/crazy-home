using Home.Shared;
using LanguageExt.Common;
using MQTT.Actions.Objects;

namespace MQTT.Actions;

public interface IMqttPlugActions {
    Task<Result<List<PlugStatusDto>>> GetAllAsync();
    Task<Result<bool>> SetStateAsync(string id, SwitchState switchState);
    Task<Result<PlugStatusDto?>> GetPlugInfoAsync(string identifier);
}
