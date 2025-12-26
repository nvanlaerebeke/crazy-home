using Home.AutoPlayer.Auth;
using Home.AutoPlayer.Dto;
using Home.Config;
using Home.Db;
using Home.Db.Repository;
using Home.Error;
using SpotifyAPI.Web;

namespace Home.AutoPlayer.Actions;

internal sealed class GetPlayList {
    private readonly SpotifyTokenStore _tokenStore;
    private readonly HomeDbContextFactory _contextFactory;

    public GetPlayList(SpotifyTokenStore tokenStore, HomeDbContextFactory contextFactory) {
        _tokenStore = tokenStore;
        _contextFactory = contextFactory;
    }

    public async Task<PlayList?> ExecuteAsync(string? name = null) {
        if (string.IsNullOrEmpty(name)) {
            name = await GetPlayListNameAsync();
        }

        if (string.IsNullOrEmpty(name)) {
            throw HomeApiException.from(ApiErrorCode.InvalidPlayListName);
        }
        
        var token = await _tokenStore.GetValidAccessToken();
        if (string.IsNullOrEmpty(token)) {
            throw HomeApiException.from(ApiErrorCode.NotFound);
        }

        var client = new SpotifyClient(token);
        return (await client.Playlists.Get(name)).ToDto();
    }

    private async Task<string?> GetPlayListNameAsync() {
        await using var work = await _contextFactory.GetAsync();
        return (await work.Settings.GetByNameAsync(Spotify.PlayListIdSettingsKey))?.Value;
    }
}

