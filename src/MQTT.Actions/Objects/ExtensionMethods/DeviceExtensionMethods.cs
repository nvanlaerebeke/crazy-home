using Home.Db.Model;

namespace MQTT.Actions.Objects.ExtensionMethods;

internal static class DeviceExtensionMethods {
    public static DeviceDto ToDto(this Device device) {
        return new() {
            Type = device.DeviceType,
            IeeeAddress = device.IeeeAddress,
            FriendlyName = device.FriendlyName
        };
    }
}

