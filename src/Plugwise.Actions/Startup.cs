using Microsoft.Extensions.DependencyInjection;
using Plugwise.Config;
using PlugwiseControl;

namespace Plugwise.Actions;

public static class Startup {
    public static void AddActions(this IServiceCollection serviceCollection, ISettings settings) {
        serviceCollection.AddSingleton<IPlugService, PlugService>();
        serviceCollection.AddPlugwise(settings.SerialPort);
        serviceCollection.AddPlugwiseCache(settings.Plugs.Select(p => p.Mac).ToList());
    }
}
