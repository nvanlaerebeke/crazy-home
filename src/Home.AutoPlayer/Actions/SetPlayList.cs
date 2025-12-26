using Home.Config;
using Home.Db;
using Home.Db.Repository;

namespace Home.AutoPlayer.Actions;

internal sealed class SetPlayList {
    private readonly HomeDbContextFactory _contextFactory;

    public SetPlayList(HomeDbContextFactory contextFactory) {
        _contextFactory = contextFactory;
    }
    public async Task ExecuteAsync(string name) {
        await using var work = await _contextFactory.GetAsync();
        await work.Settings.SetByKeyAsync(Spotify.PlayListIdSettingsKey, name);
        await work.SaveChangesAsync();
    }
}

