using Home.Db;
using Home.Db.Model;
using Home.Error;
using Home.Theming.Object;
using Home.Theming.Object.ExtensionMethods;
using Microsoft.EntityFrameworkCore;

namespace Home.Theming.Actions;

internal sealed class Add {
    private readonly HomeDbContextFactory _dbContextFactory;

    public Add(HomeDbContextFactory dbContextFactory) {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<ThemeDto> AddAsync(ThemeDto theme) {
        await using var work = await _dbContextFactory.GetAsync();
        var existing = await work.Themes.FirstOrDefaultAsync(x => x.Name == theme.Name);
        if (existing is not null) {
            throw HomeApiException.from(ApiErrorCode.Exists);
        }

        var newTheme = new Theme {
            Name = theme.Name, Primary = theme.Primary, Secondary = theme.Secondary, Tertiary = theme.Tertiary
        };
        await work.Themes.AddAsync(newTheme);
        await work.SaveChangesAsync();
        return newTheme.ToDto();
    }
}
