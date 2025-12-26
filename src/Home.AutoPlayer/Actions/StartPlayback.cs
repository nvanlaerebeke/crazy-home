using Home.AutoPlayer.Auth;
using Home.AutoPlayer.Helpers;
using Home.Config;
using Home.Db;
using Home.Db.Repository;
using Home.Error;
using SpotifyAPI.Web;

namespace Home.AutoPlayer.Actions;

internal sealed class StartPlayback {
    private readonly HomeDbContextFactory _contextFactory;
    private readonly SpotifyTokenStore _tokenStore;
    private readonly GetDevice _getDevice;

    public StartPlayback(HomeDbContextFactory contextFactory, SpotifyTokenStore tokenStore, GetDevice getDevice) {
        _contextFactory = contextFactory;
        _tokenStore = tokenStore;
        _getDevice = getDevice;
    }

    public async Task<bool> ExecuteAsync(string deviceName) {
        var device = await _getDevice.ExecuteAsync(deviceName);
        if (device is null) {
            throw HomeApiException.from(ApiErrorCode.NotFound);
        }

        var token = await _tokenStore.GetValidAccessToken();
        if (string.IsNullOrEmpty(token)) {
            throw HomeApiException.from(ApiErrorCode.NotFound);
        }

        // Accept either "spotify:playlist:..." or a raw playlist id
        var playList = await GetPlayListAsync();
        var client = new SpotifyClient(token);

        //Resume Playback
        if (!await client.Player.TransferPlayback(new PlayerTransferPlaybackRequest([device.Id]) { Play = false })) {
            throw HomeApiException.from(ApiErrorCode.UnknownError);
        }

        //Enable shuffle (separate request), do not fail if this would error out
        try {
            await client.Player.SetShuffle(new PlayerShuffleRequest(true) { DeviceId = device.Id });
        } catch (Exception ex) {
            Console.Error.WriteLine(ex);
        }

        return await client.Player.ResumePlayback(
            new PlayerResumePlaybackRequest { DeviceId = device.Id, ContextUri = playList }
        );
    }

    private async Task<string?> GetPlayListAsync() {
        await using var work = await _contextFactory.GetAsync();
        var playListSetting = await work.Settings.GetByNameAsync(Spotify.PlayListIdSettingsKey);
        return PlayListHelper.GetPlayListId(playListSetting?.Value);
    }
}
