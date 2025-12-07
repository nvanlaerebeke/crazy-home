using Home.Db;
using Home.Db.Model;
using MQTT.Actions.Message;

namespace MQTT.Actions.Objects.ExtensionMethods;

internal static class SwitchStatusExtensionMethods {
    public static SwitchStatusDto ToDto(this SwitchStatus status, Device device) {
        var switchStatus = status.State.Equals("off", StringComparison.InvariantCultureIgnoreCase)
            ? SwitchState.Off
            : SwitchState.On;
        
        var powerOnBehaviour = status.PowerOnBehavior.Equals("on", StringComparison.InvariantCultureIgnoreCase)
            ? SwitchState.On
            : SwitchState.Off;
        
        return new() {
            Id = device.IeeeAddress,
            Name = device.FriendlyName,
            SwitchState = switchStatus,
            AllowStateChange = device.AllowStateChange,
            PowerOnBehavior = powerOnBehaviour
        };
    }
}
