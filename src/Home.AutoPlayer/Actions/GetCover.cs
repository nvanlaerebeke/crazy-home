using Home.AutoPlayer.Auth;
using Home.Error;
using SpotifyAPI.Web;

namespace Home.AutoPlayer.Actions;

internal sealed class GetCover {
    private readonly SpotifyTokenStore _tokenStore;

    public GetCover(SpotifyTokenStore tokenStore) {
        _tokenStore = tokenStore;
    }
    public async Task<Uri> ExecuteAsync(string id) {
        if (string.IsNullOrEmpty(id)) {
            throw HomeApiException.from(ApiErrorCode.InvalidPlayListName);
        }
        
        var token = await _tokenStore.GetValidAccessToken();
        if (string.IsNullOrEmpty(token)) {
            throw HomeApiException.from(ApiErrorCode.NotFound);
        }
        
        var client = new SpotifyClient(token);
        //id = PlayListHelper.GetPlayListId(id);
        var playList = await client.Playlists.Get(id);
        if (playList is null) {
            throw HomeApiException.from(ApiErrorCode.NotFound);
        }
        
        return playList.Images?.Count == 0 
            ? throw HomeApiException.from(ApiErrorCode.NoCoverImage) 
            : new Uri(playList.Images!.First().Url);
    }
}

