using System.ComponentModel;
using System.Reflection;
using System.Text.Json.Serialization;
using Home.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Plugwise.Actions;
using Home.Config;
using Home.Db;
using Home.Theming;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi;
using MQTT.Actions;

namespace Home.Api;

public class Startup {
    public void Start(WebApplicationBuilder builder) {
        ConfigureServices(builder.Services);

        // optional: enable detailed logging
        builder.Logging.AddFilter("Microsoft.AspNetCore.Authentication", LogLevel.Debug);
        builder.Logging.AddFilter("Microsoft.IdentityModel", LogLevel.Debug);

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
        services.AddSwaggerGen(c => {
            // Basic doc (optional if you already have this)
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Crazyzone's Automation", Version = "v1" });

            //Use the [DisplayName] annotation on controllers to use as name in the openapi docs 
            c.TagActionsBy(apiDesc => {
                var cad = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                var ctrlType = cad?.ControllerTypeInfo;

                var displayName =
                    ctrlType?.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
                    ?? ctrlType?.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.Name
                    ?? cad?.ControllerName;

                return [displayName ?? "API"];
            });

            // Define the Bearer scheme in the openapi docs
            c.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

            c.AddSecurityRequirement(document =>
                new OpenApiSecurityRequirement { [new OpenApiSecuritySchemeReference("Bearer", document)] = [] });
        });

        //Initialize
        var settings = new SettingsProvider().Get();
        services.AddDatabase(settings);
        services.AddSingleton(settings);
        services.AddPlugwise(settings);
        services.AddMqtt(settings);
        services.AddAuth(settings);
        services.AddTheming();
        return services;
    }
}
