using Home.Theming.Actions;
using Microsoft.Extensions.DependencyInjection;

namespace Home.Theming;

public static class Setup {
    public static IServiceCollection AddTheming(this IServiceCollection services) {
        services.AddSingleton<IThemeService, ThemeService>();

        services.AddTransient<Add>();
        services.AddTransient<Delete>();
        services.AddTransient<GetBackground>();
        services.AddTransient<GetColors>();
        services.AddTransient<GetThemes>();
        services.AddTransient<SetBackground>();
        services.AddTransient<Update>();
        
        return services;
    }
}

