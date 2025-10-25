using Home.Shared;
using MQTT.Actions.Message;

namespace MQTT.Actions.Objects.ExtensionMethods;

internal static class PlugStatusExtensionMethods {
    public static PlugStatusDto ToDto(this PlugStatus plugStatus, string identifier) {
        var switchStatus = plugStatus.State.Equals("off", StringComparison.InvariantCultureIgnoreCase)
            ? SwitchState.Off
            : SwitchState.On;

        return new() {
            Identifier = identifier,
            SwitchState = switchStatus,
            Usage = switchStatus == SwitchState.Off ? 0 : plugStatus.Power,
            Current = plugStatus.Current,
            Voltage = plugStatus.Voltage,
        };
    }
}
