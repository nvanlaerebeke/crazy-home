using System.Security.Cryptography;
using Home.Auth.Actions;
using Home.Auth.Actions.Lib;
using Home.Config;
using Home.Db.Context;
using Home.Db.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Home.Auth;

public static class Setup {
    public static IServiceCollection AddAuth(this IServiceCollection services, ISettings settings) {
        services.AddSingleton<IAuthActions, AuthActions>();

        services.AddTransient<LogOn>();
        services.AddTransient<LogOut>();
        services.AddTransient<Refresh>();
        services.AddTransient<Update>();
        services.AddTransient<Create>();
        services.AddTransient<Delete>();
        
        services.AddTransient<JwtIssuer>();

        AddAuthServices(services, settings);

        CreateFirstUserIfNeeded(settings);

        return services;
    }

    private static void CreateFirstUserIfNeeded(ISettings settings) {
        using var work = new HomeDbContext(settings);
        var totalCount = work.Users.Count();
        if (totalCount != 0) {
            return;
        }

        var user = new User { UserName = "User", PasswordHash = string.Empty, RefreshTokenExpiry = null };
        user.PasswordHash = new PasswordHasher<User>().HashPassword(user, "password");
        work.Users.Add(user);
        work.SaveChanges();
    }

    private static void AddAuthServices(IServiceCollection services, ISettings settings) {
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
                o.Events = new JwtBearerEvents {
                    OnAuthenticationFailed = ctx => {
                        ctx.HttpContext.RequestServices
                            .GetRequiredService<ILoggerFactory>()
                            .CreateLogger("JWT")
                            .LogError(ctx.Exception, "JWT auth failed");
                        return Task.CompletedTask;
                    },
                    OnChallenge = ctx => {
                        var logger = ctx.HttpContext.RequestServices
                            .GetRequiredService<ILoggerFactory>()
                            .CreateLogger("JWT");
                        logger.LogWarning("JWT challenge. Error: {Error}, Desc: {Description}",
                            ctx.Error, ctx.ErrorDescription);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = ctx => {
                        var logger = ctx.HttpContext.RequestServices
                            .GetRequiredService<ILoggerFactory>()
                            .CreateLogger("JWT");
                        logger.LogInformation("JWT token validated for {Name}",
                            ctx.Principal?.Identity?.Name ?? "(no name)");
                        return Task.CompletedTask;
                    }
                };
                o.TokenValidationParameters = new TokenValidationParameters {
                    ValidIssuer = settings.Auth.Issuer,
                    ValidAudience = settings.Auth.Audience,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(key.Value)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(2)
                };
            });
        services.AddAuthorization();
    }
}
