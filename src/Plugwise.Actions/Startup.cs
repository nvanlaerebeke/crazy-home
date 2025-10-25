using Microsoft.Extensions.DependencyInjection;
using Home.Config;
using PlugwiseControl;

namespace Plugwise.Actions;

public static class Startup {
    public static void AddPlugwise(this IServiceCollection serviceCollection, ISettings settings) {
        serviceCollection.AddSingleton<IPlugService, PlugService>();
        serviceCollection.AddPlugwise(settings.Plugwise.SerialPort);

        if (settings.Plugwise.BackgroundCaching) {
            serviceCollection.AddPlugwiseCache(settings.Plugwise.Plugs.Select(p => p.Identifier).ToList());
        }
    }
}
