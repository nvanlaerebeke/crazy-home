using System.Text.Json.Serialization;
using Home.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Plugwise.Actions;
using Home.Config;
using Home.Db;
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
        app.UseAuthentication();
        app.MapControllers();
    }

    public IServiceCollection ConfigureServices(IServiceCollection services) {
        // Add services to the container.
        services.AddControllers().AddJsonOptions(o => {
            // serializes enums as strings; also affects swagger model binding in JSON bodies
            o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false));
        });


        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        //Initialize
        var settings = new SettingsProvider().Get();
        services.AddDatabase(settings);
        services.AddSingleton(settings);
        services.AddPlugwise(settings);
        services.AddMqtt(settings);
        services.AddAuth(settings);

        return services;
    }

    
}
