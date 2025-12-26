using Home.AutoPlayer.Auth;
using Home.AutoPlayer.Dto;
using LanguageExt.Common;

namespace Home.AutoPlayer;

internal sealed class SpotifyActions : ISpotifyActions {
    private readonly GetLoginUri _getLoginUri;
    private readonly HandleLoginCallback _handleLoginCallback;

    public SpotifyActions(GetLoginUri getLoginUri, HandleLoginCallback handleLoginCallback) {
        _getLoginUri = getLoginUri;
        _handleLoginCallback = handleLoginCallback;
    }

    public Result<Uri> GetLoginUri() {
        try {
            return _getLoginUri.Execute();
        } catch (Exception ex) {
            return new Result<Uri>(ex);
        }
    }

    public async Task<Result<SpotifyAuthResult>> HandleLoginCallbackAsync(string code) {
        try {
            return await _handleLoginCallback.ExecuteAsync(code);
        } catch (Exception ex) {
            return new Result<SpotifyAuthResult>(ex);
        }
    }
}
