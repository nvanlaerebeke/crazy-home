using Home.AutoPlayer.Actions;
using Home.AutoPlayer.Auth;
using Home.Config;
using Home.Db.Context;
using Home.Db.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Home.AutoPlayer;

public static class Setup {
    public static IServiceCollection AddAudioPlayerSupport(this IServiceCollection services, ISettings settings) {
        services.AddTransient<IAudioActions, AudioActions>();
        services.AddTransient<ISpotifyActions, SpotifyActions>();

        services.AddTransient<GetDevices>();
        services.AddTransient<GetDevice>();
        services.AddTransient<StartPlayback>();

        //Spotify Auth
        services.AddTransient<GetLoginUri>();
        services.AddTransient<HandleLoginCallback>();
        services.AddTransient<SpotifyTokenStore>();
        
        SetupDb(settings);
        return services;
    }
    
    private static void SetupDb(ISettings settings) {
        var keyName = Spotify.AudioApiKeySettingsName;
        using var work = new HomeDbContext(settings);
        var setting = work.Settings.GetByNameAsync(keyName).GetAwaiter().GetResult();
        
        if (setting is not null) {
            return;
        }

        work.Settings.SetByKeyAsync(keyName, Guid.NewGuid().ToString()).GetAwaiter().GetResult();
        work.SaveChanges();
    }
}
