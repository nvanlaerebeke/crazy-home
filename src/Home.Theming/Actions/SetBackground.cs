using Home.Db;
using Home.Error;
using Microsoft.EntityFrameworkCore;

namespace Home.Theming.Actions;

internal sealed class SetBackground {
    private readonly HomeDbContextFactory _dbContextFactory;
    public SetBackground(HomeDbContextFactory dbContextFactory) {
        _dbContextFactory = dbContextFactory;
    }

    public async Task SetAsync(string name, Stream backgroundImage) {
        await using var work = await _dbContextFactory.GetAsync();
        var theme = await work.Themes.FirstOrDefaultAsync(x => x.Name == name);
        if (theme is null) {
            throw HomeApiException.from(ApiErrorCode.NotFound);
        }

        var backgroundImageData = new byte[backgroundImage.Length]; 
        await backgroundImage.ReadExactlyAsync(backgroundImageData, 0, backgroundImageData.Length);

        theme.Background = backgroundImageData;
        work.Update(theme);
        await work.SaveChangesAsync();
    }
}

