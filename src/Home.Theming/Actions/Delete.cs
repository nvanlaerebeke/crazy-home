using Home.Db;
using Microsoft.EntityFrameworkCore;

namespace Home.Theming.Actions;

internal sealed class Delete {
    private readonly HomeDbContextFactory _dbContextFactory;
    public Delete(HomeDbContextFactory dbContextFactory) {
        _dbContextFactory = dbContextFactory;
    }

    public async Task DeleteAsync(string name) {
        await using var work = await _dbContextFactory.GetAsync();
        await work.Themes.Where(x => x.Name == name).ExecuteDeleteAsync();
    }
}

