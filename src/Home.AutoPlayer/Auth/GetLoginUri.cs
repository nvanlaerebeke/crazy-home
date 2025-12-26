using Home.Config;
using SpotifyAPI.Web;

namespace Home.AutoPlayer.Auth;

internal sealed class GetLoginUri {
    private readonly Uri _redirectUri; 
    private readonly string _clientId;
    
    public GetLoginUri(ISettings settings) {
        _clientId = Spotify.ClientId;
        _redirectUri = Spotify.RedirectUri;
    }
    
    public Uri Execute() {
        // Use the configured redirect URI; it MUST match Spotify dashboard exactly.
        var login = new LoginRequest(
            new Uri(_redirectUri.ToString()),
            _clientId,
            LoginRequest.ResponseType.Code) {
            Scope = [Scopes.UserReadPlaybackState, Scopes.UserModifyPlaybackState], ShowDialog = true
        };

        return login.ToUri();
    }
}

