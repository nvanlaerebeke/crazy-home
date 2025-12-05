using Home.Db;
using Microsoft.EntityFrameworkCore;

namespace Home.Auth.Actions;

internal sealed class Delete {
    private readonly HomeDbContextFactory _dbContextFactory;

    public Delete(HomeDbContextFactory dbContextFactory) {
        _dbContextFactory = dbContextFactory;
    }

    public async Task ExecuteAsync(string userName) {
        await using var work = await _dbContextFactory.GetAsync();
        var user = await work.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        if (user is null) {
            return;
        }

        work.Users.Remove(user);
        await work.SaveChangesAsync();
    }
}
