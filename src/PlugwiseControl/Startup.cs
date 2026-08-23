using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using PlugwiseControl.Actions;
using PlugwiseControl.BackgroundServices;
using PlugwiseControl.Cache;
using PlugwiseControl.Calibration;

namespace PlugwiseControl;

public static class Startup {
    public static void AddPlugwise(this IServiceCollection services, string serialPort) {
        Settings.SerialPort = serialPort;

        //Caches - these are singleton objects
        services.AddMemoryCache();
        services.AddSingleton<CircleInfoCache>();
        services.AddSingleton<UsageCache>();
        services.AddSingleton<Calibrator>();
        
        services.AddTransient<IRequestManager, RequestManager>();
        services.AddTransient<IPlugControl, PlugControl>();

        //Actions
        services.AddTransient<PlugwiseActions>();
        services.AddTransient<On>();
        services.AddTransient<Off>();
    }

    public static void AddPlugwiseCache(this IServiceCollection services, List<string> macAddresses) {
        Settings.CachedMacAddresses = macAddresses;
        services.AddHostedService<CircleInfoService>();
    }
}
