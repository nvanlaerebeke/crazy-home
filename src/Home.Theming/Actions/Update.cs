using Home.Db;
using Home.Error;
using Home.Theming.Object;
using Home.Theming.Object.ExtensionMethods;
using Microsoft.EntityFrameworkCore;

namespace Home.Theming.Actions;

internal sealed class Update {
    private readonly HomeDbContextFactory _dbContextFactory;

    public Update(HomeDbContextFactory dbContextFactory) {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<ThemeDto> UpdateAsync(ThemeDto theme) {
        await using var work = await _dbContextFactory.GetAsync();
        var dbTheme = await work.Themes.FirstOrDefaultAsync(x => x.Name == theme.Name);
        if (dbTheme is null) {
            throw HomeApiException.from(ApiErrorCode.NotFound);
        }

        dbTheme.Primary = theme.Primary;
        dbTheme.Secondary = theme.Secondary;
        dbTheme.Tertiary = theme.Tertiary;

        work.Update(dbTheme);
        await work.SaveChangesAsync();

        return dbTheme.ToDto();
    }
}
