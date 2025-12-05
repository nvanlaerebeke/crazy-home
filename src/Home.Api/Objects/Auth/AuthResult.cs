using System.Text.Json.Serialization;

namespace Home.Api.Objects.Auth;

public sealed class AuthResult {
    [JsonPropertyName("token")]
    public required string Token { get; init; }
    [JsonPropertyName("refresh_token")]
    public required string RefreshToken { get; init; }
    [JsonPropertyName("expires_at")]
    public required long  ExpiresAt { get; init; }
}

