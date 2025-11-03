using Home.Auth.Actions;
using Home.Auth.Objects;
using LanguageExt.Common;

namespace Home.Auth;

internal sealed class AuthActions : IAuthActions {
    private readonly LogOn _logOn;
    private readonly LogOut _logOut;
    private readonly Refresh _refresh;

    public AuthActions(LogOn logOn, LogOut logOut, Refresh refresh) {
        _logOn = logOn;
        _logOut = logOut;
        _refresh = refresh;
    }

    public async Task<Result<AuthResultDto>> LogOn(string userName, string Password) {
        try {
            return await _logOn.ExecuteAsync(userName, Password);
        } catch (Exception ex) {
            return new Result<AuthResultDto>(ex);
        }
    }
    
    public async Task<Result<bool>> LogOut(string userName) {
        try {
            return await _logOut.ExecuteAsync(userName);
        } catch (Exception ex) {
            return new Result<bool>(ex);
        }
    }
    
    public async Task<Result<AuthResultDto>> Refresh(string refreshToken) {
        try {
            return await _refresh.ExecuteAsync(refreshToken);
        } catch (Exception ex) {
            return new Result<AuthResultDto>(ex);
        }
    }
}

