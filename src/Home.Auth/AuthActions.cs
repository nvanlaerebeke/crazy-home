using Home.Auth.Actions;
using Home.Auth.Objects;
using LanguageExt.Common;

namespace Home.Auth;

internal sealed class AuthActions : IAuthActions {
    private readonly LogOn _logOn;
    private readonly LogOut _logOut;
    private readonly Refresh _refresh;
    private readonly Update _update;
    private readonly Create _create;
    private readonly Delete _delete;

    public AuthActions(LogOn logOn, LogOut logOut, Refresh refresh, Update update, Create create, Delete delete) {
        _logOn = logOn;
        _logOut = logOut;
        _refresh = refresh;
        _update = update;
        _create = create;
        _delete = delete;
    }

    public async Task<Result<AuthResultDto>> LogOn(string userName, string password) {
        try {
            return await _logOn.ExecuteAsync(userName, password);
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

    public async Task<Result<bool>> UpdateAsync(string userId, AuthUpdateDto authUpdate) {
        try {
            await _update.ExecuteAsync(userId, authUpdate);
            return true;
        } catch (Exception ex) {
            return new Result<bool>(ex);
        }
    }

    public async Task<Result<bool>> CreateAsync(string userName, string password) {
        try {
            await _create.ExecuteAsync(userName, password);
            return true;
        } catch (Exception ex) {
            return new Result<bool>(ex);
        }
    }

    public async Task<Result<bool>> DeleteAsync(string userName) {
        try {
            await _delete.ExecuteAsync(userName);
            return true;
        } catch (Exception ex) {
            return new Result<bool>(ex);
        }
    }
}
