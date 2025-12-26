using Home.AutoPlayer.Dto;
using Home.Config;
using SpotifyAPI.Web;

namespace Home.AutoPlayer.Auth;

internal sealed class HandleLoginCallback {
    private readonly SpotifyTokenStore _tokenStore;

    public HandleLoginCallback(SpotifyTokenStore tokenStore) {
        _tokenStore = tokenStore;
    }

    public async Task<SpotifyAuthResult> ExecuteAsync(string code) {
        if (string.IsNullOrWhiteSpace(code)) {
            throw new ArgumentException("Missing authorization code", nameof(code));
        }

        var oauth = new OAuthClient();
        var token = await oauth.RequestToken(
            new AuthorizationCodeTokenRequest(
                Spotify.ClientId,
                Spotify.ClientSecret,
                code,
                new Uri(Spotify.RedirectUri.ToString())
            ));

        // Spotify may return RefreshToken only on first consent; store it.
        // If a later callback ever omits refresh token, keep the old one.
        var existing = await _tokenStore.LoadAsync();
        if (existing is not null && string.IsNullOrWhiteSpace(token.RefreshToken))
            token.RefreshToken = existing.RefreshToken;

        await _tokenStore.SaveAsync(token);

        return new SpotifyAuthResult(
            true, !string.IsNullOrWhiteSpace(token.RefreshToken), token.ExpiresIn, token.TokenType
        );
    }
}
