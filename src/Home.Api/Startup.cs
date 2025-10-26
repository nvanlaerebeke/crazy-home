using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Plugwise.Actions;
using Home.Config;
using MQTT.Actions;

namespace Home.Api;

public class Startup {
    public void Start(WebApplicationBuilder builder) {
        ConfigureServices(builder.Services);

        var app = builder.Build();
        ConfigureApp(app);
        app.Run();
    }

    protected virtual void ConfigureApp(WebApplication app) {
        //Configure Error Handler
        app.UseExceptionHandler("/Error");
        app.UseHsts();

        //Enable swagger 
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseAuthorization();

        app.MapControllers();
    }

    public void ConfigureServices(IServiceCollection services) {
        // Add services to the container.
        services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        //Initialize
        var settings = new SettingsProvider().Get();
        services.AddSingleton(settings);
        services.AddPlugwise(settings);
        services.AddMqtt(settings);
    }
}
