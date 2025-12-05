using Home.Auth.Objects;
using LanguageExt.Common;

namespace Home.Auth;

public interface IAuthActions {
    Task<Result<AuthResultDto>> LogOn(string userName, string password);
    Task<Result<bool>> LogOut(string userName);
    Task<Result<AuthResultDto>> Refresh(string refreshToken);
    Task<Result<bool>> UpdateAsync(string userId, AuthUpdateDto authUpdate);
    Task<Result<bool>> CreateAsync(string userName, string password);
    Task<Result<bool>> DeleteAsync(string userName);
}
