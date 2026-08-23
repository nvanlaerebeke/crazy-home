using Home.AutoPlayer.Auth;
using Home.AutoPlayer.Dto;
using Home.Error;
using LanguageExt.Common;
using SpotifyAPI.Web;

namespace Home.AutoPlayer.Actions;

internal sealed class GetUserPlaylists {
    private readonly SpotifyTokenStore _tokenStore;

    public GetUserPlaylists(SpotifyTokenStore tokenStore) {
        _tokenStore = tokenStore;
    }

    public async Task<Result<List<PlayList>>> GetAsync() {
        var token = await _tokenStore.GetValidAccessToken();
        if (string.IsNullOrEmpty(token)) {
            throw HomeApiException.from(ApiErrorCode.UnAuthorized);
        }

        var client = new SpotifyClient(token);
        return (await client.Playlists.CurrentUsers()).Items?.Select(p => p.ToDto()).ToList() ?? [];
    }
}
