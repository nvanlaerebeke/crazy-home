using LanguageExt.Common;
using MQTT.Actions.Objects;

namespace MQTT.Actions;

public interface IMqttDeviceActions {
    Task<Result<List<DeviceDto>>> GetAllAsync();
    Task<Result<bool>> SetFriendlyNameAsync(string ieeeAddress, string friendlyName);
    Task<Result<bool>> RemoveAsync(string ieeeAddress);
}
