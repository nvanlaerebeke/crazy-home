using System.Security.Cryptography;
using System.Text;
using Home.Auth.Actions;
using Home.Auth.Actions.Lib;
using Home.Config;
using Home.Db.Context;
using Home.Db.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Home.Auth;

public static class Setup {
    public static IServiceCollection AddAuth(this IServiceCollection services, ISettings settings) {
        services.AddSingleton<IAuthActions, AuthActions>();

        services.AddTransient<LogOn>();
        services.AddTransient<LogOut>();
        services.AddTransient<Refresh>();

        services.AddTransient<JwtIssuer>();
        
        return AddAuthServices(services, settings);
    }

    private static IServiceCollection AddAuthServices(IServiceCollection services, ISettings settings) {
        //Get the password signing key
        using var work = new HomeDbContext(settings);
        var key = work.Settings.FirstOrDefault(x => x.Key == "PasswordSignKey");
        if (key is null) {
            //generate key
            key = new Setting {
                Key = "PasswordSignKey",
                Value = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)) // = 256 bit signing key
            };
            work.Settings.Add(key);
            work.SaveChanges();
        }

        //configure the services
        services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o => {
                o.TokenValidationParameters = new TokenValidationParameters {
                    ValidIssuer = settings.Auth.Issuer,
                    ValidAudience = settings.Auth.Audience,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key.Value)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(2)
                };
            });
        services.AddAuthorization();

        return services;
    }
}
