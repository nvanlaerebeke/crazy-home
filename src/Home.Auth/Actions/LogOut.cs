using Home.Db;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;

namespace Home.Auth.Actions;

internal sealed class LogOut {
    private readonly HomeDbContextFactory _dbContextFactory;

    public LogOut(HomeDbContextFactory dbContextFactory) {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Result<bool>> ExecuteAsync(string userName) {
        await using var work = await _dbContextFactory.GetAsync();
        var user = await work.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        if (user is null) {
            return true;
        }

        user.RefreshToken = string.Empty;
        user.RefreshTokenExpiry = DateTime.UtcNow;
        work.Users.Update(user);
        await work.SaveChangesAsync();

        return true;
    }
}
