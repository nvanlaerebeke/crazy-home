using Home.Db;
using Microsoft.EntityFrameworkCore;

namespace Home.Theming.Actions;

internal sealed class GetThemes {
    private readonly HomeDbContextFactory _dbContextFactory;
    public GetThemes(HomeDbContextFactory dbContextFactory) {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<List<string>> GetAsync() {
        await using var work = await _dbContextFactory.GetAsync();
        return await work.Themes.Select(x => x.Name).ToListAsync();
    }
}

