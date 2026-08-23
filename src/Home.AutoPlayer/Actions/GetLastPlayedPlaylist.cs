using Home.AutoPlayer.Auth;
using Home.Error;
using SpotifyAPI.Web;

namespace Home.AutoPlayer.Actions;

internal sealed class GetLastPlayedPlaylist {
    private readonly SpotifyTokenStore _tokenStore;

    public GetLastPlayedPlaylist(SpotifyTokenStore tokenStore) {
        _tokenStore = tokenStore;
    }

    /// <summary>
    /// Tries to find the last played playlist.
    /// 1) If currently playing context is a playlist -> returns it.
    /// 2) Else scans recently played items for a playlist context -> returns it.
    /// Returns null if no playlist context can be determined.
    /// </summary>
    public async Task<FullPlaylist?> GetAsync(CancellationToken ct = default) {
        //Create the Spotify client
        var token = await _tokenStore.GetValidAccessToken();
        if (string.IsNullOrEmpty(token)) {
            throw HomeApiException.from(ApiErrorCode.UnAuthorized);
        }

        var client = new SpotifyClient(token);

        // (1) Prefer the current playback context if it is a playlist
        var currentPlaylistId = await TryGetCurrentPlaylistIdAsync(client, ct);
        if (!string.IsNullOrEmpty(currentPlaylistId)) {
            return await client.Playlists.Get(currentPlaylistId, ct);
        }

        // (2) Fall back to recently played playlist
        var recent = await client.Player.GetRecentlyPlayed(new PlayerRecentlyPlayedRequest {Limit = Math.Clamp(1, 1, 50)}, ct);

        if (recent.Items == null || recent.Items.Count == 0) {
            return null;
        }

        foreach (var item in recent.Items) {
            ct.ThrowIfCancellationRequested();

            var ctxUri = item.Context.Uri;
            var playlistId = TryParsePlaylistIdFromUri(ctxUri);

            if (!string.IsNullOrEmpty(playlistId)) {
                return await client.Playlists.Get(playlistId, ct);
            }
        }

        return null;
    }

    private static async Task<string?> TryGetCurrentPlaylistIdAsync(SpotifyClient client, CancellationToken ct) {
        // Player.GetCurrentPlayback covers both playing and paused state
        var playback = await client.Player.GetCurrentPlayback(new PlayerCurrentPlaybackRequest(), ct);
        var ctxUri = playback.Context?.Uri;
        return TryParsePlaylistIdFromUri(ctxUri);
    }

    private static string? TryParsePlaylistIdFromUri(string? uri) {
        // Typical forms:
        // spotify:playlist:<id>
        // https://open.spotify.com/playlist/<id>
        if (string.IsNullOrWhiteSpace(uri))
            return null;

        const string spotifyPrefix = "spotify:playlist:";
        if (uri.StartsWith(spotifyPrefix, StringComparison.OrdinalIgnoreCase))
            return uri[spotifyPrefix.Length..].Trim();

        const string openPrefix = "https://open.spotify.com/playlist/";
        if (!uri.StartsWith(openPrefix, StringComparison.OrdinalIgnoreCase)) {
            return null;
        }

        var rest = uri[openPrefix.Length..];
        var id = rest.Split('?', '#', '/')[0];
        return string.IsNullOrWhiteSpace(id) ? null : id.Trim();
    }
}
