using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Plugwise.Actions;
using Plugwise.Config;

namespace Plugwise.Api;

public static class Startup {
    public static void Start(WebApplicationBuilder builder) {
        ConfigureServices(builder.Services);

        var app = builder.Build();
        ConfigureApp(app);
        app.Run();
    }

    private static void ConfigureApp(WebApplication app) {
        //Configure Error Handler
        app.UseExceptionHandler("/Error");
        app.UseHsts();

        //Enable swagger 
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseAuthorization();

        app.MapControllers();
        app.MapGet("/", () => "Hello World");
    }

    private static void ConfigureServices(IServiceCollection services) {
        // Add services to the container.
        services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        //Initialize
        var settings = new SettingsProvider().Get();
        services.AddSingleton(settings);
        services.AddActions(settings);
    }
}
