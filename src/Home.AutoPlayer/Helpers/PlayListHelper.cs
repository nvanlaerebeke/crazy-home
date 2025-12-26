namespace Home.AutoPlayer.Helpers;

internal static class PlayListHelper {
    private const string PlayListIdPrefix = "spotify:playlist:";
    /// <summary>
    /// Normalizes the playlist id to be used with Spotify API requests
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetPlayListId(string? name) {
        if (string.IsNullOrEmpty(name)) {
            return string.Empty;
        }
        
        return name.StartsWith(PlayListIdPrefix, StringComparison.OrdinalIgnoreCase)
            ? name
            : $"{PlayListIdPrefix}{name}";
    }
}

