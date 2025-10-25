using LanguageExt.Common;
using MQTT.Actions.Objects;

namespace MQTT.Actions;

public interface IMqttPlugActions {
    Task<Result<List<PlugStatusDto>>> GetAllAsync();
    Task<Result<bool>> SetOnAsync(string id);
    Task<Result<bool>> SetOffAsync(string id);
    Task<Result<PlugStatusDto?>> GetPlugInfoAsync(string identifier);
}
