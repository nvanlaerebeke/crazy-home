using SpotifyAPI.Web;

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
    public static PlayList ToDto(this FullPlaylist playlist) {
        return new() {
            Collaborative = playlist.Collaborative,
            Description = playlist.Description,
            ExternalUrls = playlist.ExternalUrls,
            Href = playlist.Href,
            Id = playlist.Id,
            Name = playlist.Name,
            Public = playlist.Public,
            SnapshotId = playlist.SnapshotId,
            Type = playlist.Type,
            Uri = playlist.Uri
        };
    }
}

