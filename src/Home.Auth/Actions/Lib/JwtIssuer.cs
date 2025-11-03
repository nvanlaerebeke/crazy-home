using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Home.Config;
using Home.Db;
using Home.Db.Model;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Home.Auth.Actions.Lib;

internal class JwtIssuer {
    private readonly ISettings _settings;
    private readonly HomeDbContextFactory _dbContextFactory;

    public JwtIssuer(ISettings settings, HomeDbContextFactory dbContextFactory) {
        _settings = settings;
        _dbContextFactory = dbContextFactory;
    }

    public string CreateRefreshTokenRaw() =>
        WebEncoders.Base64UrlEncode(RandomNumberGenerator.GetBytes(32)); // 256-bit, URL-safe

    public string HashRefreshToken(string raw) =>
        Convert.ToBase64String(SHA256.HashData(WebEncoders.Base64UrlDecode(raw)));

    public string Issue(
        string userId,
        string username,
        string issuer,
        string audience,
        byte[] signingKey // 256-bit random key (HS256)
    ) {
        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
            issuer,
            audience,
            new List<Claim> {
                new(JwtRegisteredClaimNames.Sub, userId), new(JwtRegisteredClaimNames.UniqueName, username),
            },
            DateTime.UtcNow,
            DateTime.UtcNow.Add(TimeSpan.FromMinutes(15)),
            new SigningCredentials(
                new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256
            )
        ));
    }

    public async Task<string?> CreateTokenAsync(User user) {
        await using var work = await _dbContextFactory.GetAsync();
        var key = await work.Settings.FirstOrDefaultAsync(x => x.Key == "PasswordSignKey");
        if (key is null) {
            return null;
        }

        // issue short-lived JWT
        var token = Issue(
            user.Id,
            user.UserName,
            _settings.Auth.Issuer,
            _settings.Auth.Audience,
            Convert.FromBase64String(key.Value)
        );
        return (token);
    }

    public async Task<string> SetRefreshTokenAsync(User user) {
        await using var work = await _dbContextFactory.GetAsync();

        var refreshTokenRaw = CreateRefreshTokenRaw();
        var refreshTokenHash = HashRefreshToken(refreshTokenRaw);
        user.RefreshToken = refreshTokenHash;
        work.Users.Update(user);
        await work.SaveChangesAsync();

        return refreshTokenRaw;
    }
}
