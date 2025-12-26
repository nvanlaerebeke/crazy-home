namespace Home.Config;

public sealed class Spotify {
    private const string ClientIdEnvVar = "SPOTIFY_CLIENT_ID";
    private const string ClientSecretEnvVar = "SPOTIFY_CLIENT_SECRET";
    public static string ClientId => Environment.GetEnvironmentVariable(ClientIdEnvVar) ?? string.Empty;
    public static string ClientSecret => Environment.GetEnvironmentVariable(ClientSecretEnvVar) ?? string.Empty;
    public static Uri RedirectUri => new("https://home.crazyzone.be/audio/spotify/login/callback");
    public const string AudioApiKeySettingsName = "AudioApiKey";
    public const string PlayListIdSettingsKey = "Spotify.PlayListId";
}

