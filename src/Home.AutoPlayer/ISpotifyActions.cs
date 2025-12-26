using Home.AutoPlayer.Dto;
using LanguageExt.Common;

namespace Home.AutoPlayer;

public interface ISpotifyActions {
    /// <summary>
    /// Returns the Spotify authorization URL to redirect the admin user to.
    /// </summary>
    /// <returns></returns>
    Result<Uri> GetLoginUri();

    /// <summary>
    ///  Exchanges the callback "code" for tokens and stores them (refresh token included).
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    Task<Result<SpotifyAuthResult>> HandleLoginCallbackAsync(string code);
}
