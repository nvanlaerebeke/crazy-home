namespace Home.AutoPlayer.Dto;

public sealed class SpotifyAuthResult {
    public SpotifyAuthResult(bool ok, bool hasRefreshToken, int expiresIn, string tokenType) {
        Ok = ok;
        HasRefreshToken = hasRefreshToken;
        ExpiresIn = expiresIn;
        TokenType = tokenType;
    }

    public bool Ok { get; init; }
    public bool HasRefreshToken { get; init; }
    public int ExpiresIn { get; init; }
    public string TokenType { get; init; }
}
