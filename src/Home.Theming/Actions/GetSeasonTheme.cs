using Home.Db;
using Home.Error;
using Home.Theming.Object;
using Home.Theming.Object.ExtensionMethods;
using Microsoft.EntityFrameworkCore;

namespace Home.Theming.Actions;

internal sealed class GetSeasonTheme {
    private readonly HomeDbContextFactory _dbContextFactory;
    private readonly SeasonService _seasonService;

    public GetSeasonTheme(HomeDbContextFactory dbContextFactory, SeasonService seasonService) {
        _dbContextFactory = dbContextFactory;
        _seasonService = seasonService;
    }

    public async Task<ThemeDto?> GetAsync() {
        var season = _seasonService.GetCurrentSeason();

        await using var work = await _dbContextFactory.GetAsync();
        var theme = await work.Themes.FirstOrDefaultAsync(x => x.Name == season.ToString());
        return theme is null ? throw HomeApiException.from(ApiErrorCode.NotFound) : theme.ToDto();
    }
}
