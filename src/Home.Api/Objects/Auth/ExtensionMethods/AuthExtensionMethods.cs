using Home.Auth.Objects;

namespace Home.Api.Objects.Auth.ExtensionMethods;

internal static class AuthExtensionMethods {
    public static AuthResult ToApiObject(this AuthResultDto authResult) {
        return new() {
            Token = authResult.AccessToken,
            RefreshToken = authResult.RefreshToken,
            ExpiresAt = new DateTimeOffset(authResult.ExpiresAt).ToUnixTimeSeconds()
        };
    }
}
