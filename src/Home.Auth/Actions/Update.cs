using Home.Auth.Objects;
using Home.Db;
using Home.Db.Model;
using Home.Error;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Home.Auth.Actions;

internal sealed class Update {
    private readonly HomeDbContextFactory _dbContextFactory;
    private readonly IPasswordHasher<User> _passwordHasher;

    public Update(HomeDbContextFactory dbContextFactory, IPasswordHasher<User>  passwordHasher) {
        _dbContextFactory = dbContextFactory;
        _passwordHasher = passwordHasher;
    }
    public async Task ExecuteAsync(string userId, AuthUpdateDto authUpdate) {
        await using var work = await _dbContextFactory.GetAsync();
        var user = await work.Users.FirstOrDefaultAsync(x => x.UserName == userId);
        if (user is null) {
            throw HomeApiException.from(ApiErrorCode.NotFound);
        }
        
        user.PasswordHash =  _passwordHasher.HashPassword(user, authUpdate.Password);
        work.Users.Update(user);
        await work.SaveChangesAsync();
    }
}

