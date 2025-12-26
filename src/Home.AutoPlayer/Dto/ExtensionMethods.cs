namespace Home.AutoPlayer.Dto;

internal static class ExtensionMethods {
    public static Device ToDto(this SpotifyAPI.Web.Device device) {
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

