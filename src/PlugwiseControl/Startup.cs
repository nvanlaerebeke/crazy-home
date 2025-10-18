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
        
        services.AddMemoryCache();

        services.AddSingleton<CircleInfoCache>();
        services.AddSingleton<UsageCache>();
        services.AddSingleton<RequestManager>();
        services.AddSingleton<Calibrator>();
        services.AddSingleton<IPlugControl, PlugControl>();
        
        //Actions
        services.AddSingleton<PlugwiseActions>();
        services.AddSingleton<On>();
        services.AddSingleton<Off>();
        
    }
    
    public static void AddPlugwiseCache(this IServiceCollection services, List<string> macAddresses) {
        Settings.CachedMacAddresses = macAddresses;
        
        services.AddHostedService<CircleInfoService>();
    }
}
