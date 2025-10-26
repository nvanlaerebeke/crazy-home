using Home.Config;
using Home.Db.Context;
using Microsoft.EntityFrameworkCore.Design;

namespace Home.Db;

public sealed class DesignHomeDbContextFactory : IDesignTimeDbContextFactory<HomeDbContext> {
    public HomeDbContext CreateDbContext(string[] args) {
        return new HomeDbContext(new SettingsProvider().Get());
    }
}
