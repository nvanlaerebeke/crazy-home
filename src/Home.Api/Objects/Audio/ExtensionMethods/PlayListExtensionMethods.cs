namespace Home.Api.Objects.Audio.ExtensionMethods;

internal static class PlayListExtensionMethods {
    public static PlayList? ToApiObject(this AutoPlayer.Dto.PlayList? playList) {
        if (playList == null) {
            return null;
        }
        return new() {
            Collaborative = playList.Collaborative,
            Description = playList.Description,
            ExternalUrls = playList.ExternalUrls,
            Href = playList.Href,
            Id = playList.Id,
            Name = playList.Name,
            Public = playList.Public,
            SnapshotId = playList.SnapshotId,
            Type = playList.Type,
            Uri = playList.Uri
        };
    }
}

