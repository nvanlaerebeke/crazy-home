using Home.Db;
using Home.Db.Model;
using Home.Error;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Home.Auth.Actions;

internal sealed class Create {
    private readonly HomeDbContextFactory _dbContextFactory;
    private readonly IPasswordHasher<User> _passwordHasher;

    public Create(HomeDbContextFactory dbContextFactory, IPasswordHasher<User> passwordHasher) {
        _dbContextFactory = dbContextFactory;
        _passwordHasher = passwordHasher;
    }

    public async Task<User> ExecuteAsync(string userName, string password) {
        await using var work = await _dbContextFactory.GetAsync();
        var user = await work.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        if (user is not null) {
            throw HomeApiException.from(ApiErrorCode.Exists);
        }

        user = new User {
            UserName = userName, PasswordHash = string.Empty, RefreshToken = string.Empty, RefreshTokenExpiry = null
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, password);
        work.Users.Add(user);
        await work.SaveChangesAsync();
        return user;
    }
}
