using Home.Db;
using Home.Error;
using Home.Theming.Object;
using Microsoft.EntityFrameworkCore;

namespace Home.Theming.Actions;

internal sealed class GetBackground {
    private const string ContentType = "application/octet-stream";
    private readonly HomeDbContextFactory _dbContextFactory;

    public GetBackground(HomeDbContextFactory dbContextFactory) {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<ThemeBackground> GetAsync(string name) {
        await using var work = await _dbContextFactory.GetAsync();
        var theme = await work.Themes.FirstOrDefaultAsync(x => x.Name == name);

        if (theme is null || theme.Background.Length == 0) {
            throw HomeApiException.from(ApiErrorCode.NotFound);
        }

        var lastMod = theme.LastUpdated;
        var etag = $"W/\"{theme.Background.Length:x}-{lastMod.Ticks:x}\"";
        
        var stream = new MemoryStream(theme.Background, 0, theme.Background.Length, false, false);
        stream.Position = 0;

        // Stream is seekable -> we can allow Range
        return new ThemeBackground {
            Content = stream,
            ContentType = ContentType,
            Length = stream.Length,
            ETag = etag,
            LastModified = new DateTimeOffset(lastMod, TimeSpan.Zero),
            SupportsRanges = stream.CanSeek
        };
    }
}
