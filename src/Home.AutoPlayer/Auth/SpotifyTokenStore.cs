using System.Text.Json;
using Home.Config;
using Home.Db;
using Home.Db.Repository;
using Home.Error;
using SpotifyAPI.Web;

namespace Home.AutoPlayer.Auth;

public sealed class SpotifyTokenStore {
    private const string SpotifyTokenKey = "SpotifyToken";
    private readonly HomeDbContextFactory _contextFactory;
    private readonly SemaphoreSlim _refreshLock = new(1, 1);

    public SpotifyTokenStore(HomeDbContextFactory contextFactory) {
        _contextFactory = contextFactory;
    }

    public async Task SaveAsync(AuthorizationCodeTokenResponse token) {
        await using var work = await _contextFactory.GetAsync();
        await work.Settings.SetByKeyAsync(
            SpotifyTokenKey,
            JsonSerializer.Serialize(token, new JsonSerializerOptions { WriteIndented = true })
        );
        await work.SaveChangesAsync();
    }

    public async Task<AuthorizationCodeTokenResponse?> LoadAsync() {
        await using var work = await _contextFactory.GetAsync(CancellationToken.None);
        var setting = await work.Settings.GetByNameAsync(SpotifyTokenKey);
        return setting is null ? null : JsonSerializer.Deserialize<AuthorizationCodeTokenResponse>(setting.Value);
    }

    public async Task<string> GetValidAccessToken() {
        var currentToken = await LoadAsync();
        if (currentToken is null) {
            throw HomeApiException.from(ApiErrorCode.UnAuthorized);
        }

        if (currentToken.CreatedAt.AddSeconds(currentToken.ExpiresIn).AddMinutes(-1) > DateTime.UtcNow) {
            return currentToken.AccessToken;
        }

        //refresh token
        await _refreshLock.WaitAsync();
        try {
            // another request might have refreshed while we waited
            currentToken = await LoadAsync();
            if (currentToken is null) {
                throw HomeApiException.from(ApiErrorCode.UnAuthorized);
            }

            if (!IsExpiringSoon(currentToken, TimeSpan.FromMinutes(1))) {
                return currentToken.AccessToken;
            }

            if (string.IsNullOrWhiteSpace(currentToken.RefreshToken)) {
                throw HomeApiException.from(ApiErrorCode.UnAuthorized); // no way to refresh
            }

            try {
                var refreshed = await new OAuthClient().RequestToken(new AuthorizationCodeRefreshRequest(
                    Spotify.ClientId, 
                    Spotify.ClientSecret, 
                    currentToken.RefreshToken)
                );
                currentToken.AccessToken = refreshed.AccessToken;
                currentToken.ExpiresIn = refreshed.ExpiresIn;
                currentToken.CreatedAt = refreshed.CreatedAt;
                
                await SaveAsync(currentToken);
                
                return currentToken.AccessToken;
            } catch (APIException ex) {
                // Typical: invalid_grant / invalid_client -> treat as logged out
                Console.Error.WriteLine(ex);
                throw HomeApiException.from(ApiErrorCode.UnAuthorized);
            }
        } finally {
            _refreshLock.Release();
        }
    }

    private static bool IsExpiringSoon(AuthorizationCodeTokenResponse t, TimeSpan earlyRefresh) {
        var expiresAt = t.CreatedAt.AddSeconds(t.ExpiresIn);
        return DateTimeOffset.UtcNow >= (expiresAt - earlyRefresh);
    }
}
