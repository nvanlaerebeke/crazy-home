using Home.AutoPlayer.Auth;
using Home.AutoPlayer.Helpers;
using Home.Error;
using SpotifyAPI.Web;

namespace Home.AutoPlayer.Actions;

internal sealed class PlayLikedSongs {
    private const int MaxTracks = 300;
    private const int PageSize = 50; //Limited by Spotify API
    private readonly SpotifyTokenStore _tokenStore;

    public PlayLikedSongs(SpotifyTokenStore tokenStore) {
        _tokenStore = tokenStore;
    }

    public async Task<bool> PlayAsync() {
        var token = await _tokenStore.GetValidAccessToken();
        if (string.IsNullOrEmpty(token))
            throw HomeApiException.from(ApiErrorCode.UnAuthorized);

        var client = new SpotifyClient(token);

        // 0 ) Get the device the playback will start on
        var deviceId = (await GetCurrentOrFirstAsync(client))?.Id;
        if (string.IsNullOrEmpty(deviceId)) {
            throw HomeApiException.from(ApiErrorCode.NoSpotifyDeviceFound);
        }

        // 1) Load all liked tracks
        var tracks = await LoadRandomLikedTracksAsync(client);
        if (tracks.Count == 0) {
            return false;
        }

        // 2) Shuffle in-place (use Fisher–Yates; your Shuffle() is fine if it's unbiased)
        tracks.Shuffle();

        // Optional: enable shuffle mode in Spotify player (separate from your list shuffle)
        try {
            await client.Player.SetShuffle(new PlayerShuffleRequest(true) {DeviceId = deviceId});
        } catch (Exception) {
            //ignore
        }

        // 3) Start playback with the first PageSize URIs
        var batch = tracks.Take(MaxTracks).ToList();
        tracks.RemoveRange(0, batch.Count);

        await client.Player.ResumePlayback(new PlayerResumePlaybackRequest {Uris = batch, DeviceId = deviceId});
        return true;
    }

    private static async Task<Device?> GetCurrentOrFirstAsync(SpotifyClient client) {
        // GET /v1/me/player/devices
        var devices = await client.Player.GetAvailableDevices();

        if (devices.Devices.Count == 0) {
            return null;
        }

        // "Current" device = the active device (if any)
        var active = devices.Devices.FirstOrDefault(d => d.IsActive);
        // No active device -> just pick the first available
        return active ?? devices.Devices[0];
    }

    // Loads up to maxTracks liked tracks from a random position in the user's library.
    private static async Task<List<string>> LoadRandomLikedTracksAsync(
        SpotifyClient client,
        CancellationToken ct = default
    ) {
        try {
            // First call: get total (and optionally 1 item)
            var first = await client.Library.GetTracks(new LibraryTracksRequest {Limit = 1, Offset = 0}, ct);

            var total = first.Total ?? 0;
            if (total <= 0) {
                return [];
            }

            var take = Math.Min(MaxTracks, total);

            // Pick a random start so that [start...start+take) fits in [0...total)
            var maxStart = total - take;
            var start = maxStart > 0 ? Random.Shared.Next(0, maxStart + 1) : 0;

            // Align to page boundary to minimize calls (optional but nice)
            start = (start / PageSize) * PageSize;

            var result = new List<string>(capacity: take);

            // Fetch pages starting at 'start' until we have 'take'
            for (int offset = start; result.Count < take; offset += PageSize) {
                ct.ThrowIfCancellationRequested();
                var page = await client.Library.GetTracks(new LibraryTracksRequest {Limit = PageSize, Offset = offset}, ct);
                if (page.Items == null || page.Items.Count == 0) {
                    break;
                }

                foreach (var uri in page.Items.Select(item => item.Track.Uri)) {
                    if (!string.IsNullOrWhiteSpace(uri)) {
                        result.Add(uri);
                    }

                    if (result.Count >= take) {
                        break;
                    }
                }

                // Safety: don't go beyond the total
                if (offset + PageSize >= total) {
                    break;
                }
            }

            return result;
        } catch (Exception ex) {
            await Console.Error.WriteLineAsync(ex.ToString());
            throw;
        }
    }
}
