using Home.Db;
using Home.Error;
using Home.Theming.Object;
using Home.Theming.Object.ExtensionMethods;
using Microsoft.EntityFrameworkCore;

namespace Home.Theming.Actions;

internal sealed class GetColors {
    private readonly HomeDbContextFactory _dbContextFactory;
    public GetColors(HomeDbContextFactory dbContextFactory) {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<ThemeDto> GetAsync(string name) {
        await using var work = await _dbContextFactory.GetAsync();
        var theme = await work.Themes.FirstOrDefaultAsync(x => x.Name == name);
        if (theme is null) {
            throw HomeApiException.from(ApiErrorCode.NotFound);
        }

        return theme.ToDto();
    }
}

