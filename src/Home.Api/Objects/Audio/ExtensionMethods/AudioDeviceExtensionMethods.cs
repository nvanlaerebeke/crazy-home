using Home.AutoPlayer.Dto;

namespace Home.Api.Objects.Audio.ExtensionMethods;

internal static class AudioDeviceExtensionMethods {
    public static AudioDevice? ToApiObject(this Device? device) {
        if (device is null) {
            return null;
        }
        return new() {
            Id = device.Id,
            IsActive = device.IsActive,
            IsPrivateSession = device.IsPrivateSession,
            IsRestricted = device.IsRestricted,
            Name = device.Name,
            SupportsVolume = device.SupportsVolume,
            Type = device.Type,
            VolumePercent = device.VolumePercent
        };
    }
}
