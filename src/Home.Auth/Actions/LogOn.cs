using Home.Auth.Actions.Lib;
using Home.Auth.Objects;
using Home.Db;
using Home.Db.Model;
using Home.Error;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Home.Auth.Actions {
    internal sealed class LogOn {
        private readonly HomeDbContextFactory _dbContextFactory;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly JwtIssuer _jwtIssuer;

        public LogOn(
            HomeDbContextFactory dbContextFactory,
            IPasswordHasher<User> passwordHasher,
            JwtIssuer jwtIssuer
        ) {
            _dbContextFactory = dbContextFactory;
            _passwordHasher = passwordHasher;
            _jwtIssuer = jwtIssuer;
        }

        public async Task<Result<AuthResultDto>> ExecuteAsync(string userName, string password) {
            await using var work = await _dbContextFactory.GetAsync();
            var user = await work.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user is null) {
                return new Result<AuthResultDto>(new UnauthorizedAccessException());
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Failed) {
                return new Result<AuthResultDto>(new UnauthorizedAccessException());
            }

            var token = await _jwtIssuer.CreateTokenAsync(user);
            if (string.IsNullOrEmpty(token)) {
                return new Result<AuthResultDto>(HomeApiException.from(ApiErrorCode.UnknownError));
            }

            return new Result<AuthResultDto>(new AuthResultDto {
                AccessToken = token,
                RefreshToken = await _jwtIssuer.SetRefreshTokenAsync(user),
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            });
        }
    }
}
