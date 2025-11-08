using Home.Auth.Actions.Lib;
using Home.Auth.Objects;
using Home.Db;
using Home.Error;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;

namespace Home.Auth.Actions;

internal sealed class Refresh {
    private readonly JwtIssuer _jwtIssuer;
    private readonly HomeDbContextFactory _dbContextFactory;

    public Refresh(JwtIssuer jwtIssuer, HomeDbContextFactory dbContextFactory) {
        _jwtIssuer = jwtIssuer;
        _dbContextFactory = dbContextFactory;
    }

    public async Task<Result<AuthResultDto>> ExecuteAsync(string refreshToken) {
        var currentRefreshToken = _jwtIssuer.HashRefreshToken(refreshToken);

        await using var work = await _dbContextFactory.GetAsync();
        var user = await work.Users.FirstOrDefaultAsync(x =>
            x.RefreshToken == currentRefreshToken && x.RefreshTokenExpiry > DateTime.UtcNow
        );
        if (user is null) {
            return new Result<AuthResultDto>(HomeApiException.from(ApiErrorCode.NotFound));
        }

        var token = await _jwtIssuer.CreateTokenAsync(user);
        if (string.IsNullOrEmpty(token)) {
            return new Result<AuthResultDto>(HomeApiException.from(ApiErrorCode.UnknownError));
        }

        return new AuthResultDto {
            AccessToken = token,
            RefreshToken = await _jwtIssuer.SetRefreshTokenAsync(user),
            ExpiresAt = DateTime.UtcNow.AddMinutes(15)
        };
    }
}
