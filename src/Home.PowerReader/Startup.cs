using Microsoft.Extensions.DependencyInjection;

namespace Home.PowerReader;

public static class Startup {
    public static void AddPowerReader(this IServiceCollection services) {
        services.AddHostedService<PollBackgroundTask>();
        
        services.AddHttpClient<IPowerReaderClient, PowerReaderClient>(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(10);
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            client.DefaultRequestHeaders.UserAgent.ParseAdd("HomeController/1.0");
        });

    }
}
