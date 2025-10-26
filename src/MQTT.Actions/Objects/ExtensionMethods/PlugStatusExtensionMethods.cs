using Home.Db.Model;
using Home.Shared;
using MQTT.Actions.Message;

namespace MQTT.Actions.Objects.ExtensionMethods;

internal static class PlugStatusExtensionMethods {
    public static PlugStatusDto ToDto(this PlugStatus plugStatus, Device device) {
        var switchStatus = plugStatus.State.Equals("off", StringComparison.InvariantCultureIgnoreCase)
            ? SwitchState.Off
            : SwitchState.On;

        return new() {
            Id = device.IeeeAddress,
            Name = device.FriendlyName,
            SwitchState = switchStatus,
            Usage = switchStatus == SwitchState.Off ? 0 : plugStatus.Power,
            Current = plugStatus.Current,
            Voltage = plugStatus.Voltage,
        };
    }
}
