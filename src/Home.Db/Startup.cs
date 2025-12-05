using Home.Config;
using Home.Db.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Home.Db;

public static class Startup {
    public static IServiceCollection AddDatabase(this IServiceCollection services, ISettings settings) {
        if (!Directory.Exists(settings.ConfigDirectory)) {
            Directory.CreateDirectory(settings.ConfigDirectory);
        }

        // Scoped DbContext with pooling for web requests
        services.AddDbContextPool<HomeDbContext>((_, o) => {
            o.UseSqlite($"Data Source={settings.ConfigDirectory}/home.sqlite");
        });

        // Factory for singletons/background services
        services.AddDbContextFactory<HomeDbContext>((_, o) => {
            o.UseSqlite($"Data Source={settings.ConfigDirectory}/home.sqlite");
        });
        
        services.AddSingleton<HomeDbContextFactory>(s =>
            new HomeDbContextFactory(s.GetRequiredService<IDbContextFactory<HomeDbContext>>())
        );

        SetupDb(settings);
        return services;
    }

    private static void SetupDb(ISettings settings) {
        using var work = new HomeDbContext(settings);
        work.Database.Migrate();
        work.ConfigurePragmasAsync().GetAwaiter().GetResult();
    }
}
