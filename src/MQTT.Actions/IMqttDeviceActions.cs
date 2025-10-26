using LanguageExt.Common;

namespace MQTT.Actions;

public interface IMqttDeviceActions {
    Task<Result<bool>> SetFriendlyNameAsync(string ieeeAddress, string friendlyName);
}
