namespace Home.Config;

public sealed class Spotify {
    public static string ClientId => "bc8793a90f9b47789ba38fc30dd9fb8d";
    public static string ClientSecret => "929d6b1f115c4612b195fb0a8d19ed3c";
    public static Uri RedirectUri => new("https://home.crazyzone.be/audio/spotify/login/callback");
    public static string PlayListId => "5BYJ6J3hM0LSrWKJbWVubu?si=d8c6697272f249a9";

    public const string AudioApiKeySettingsName = "AudioApiKey";
}

