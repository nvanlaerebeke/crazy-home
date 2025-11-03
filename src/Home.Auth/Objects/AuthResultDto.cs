namespace Home.Auth.Objects;

public sealed class AuthResultDto {
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
    public required DateTime ExpiresAt { get; init; }
}

