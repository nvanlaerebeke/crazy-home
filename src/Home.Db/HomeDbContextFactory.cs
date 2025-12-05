using Home.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Home.Db;

public sealed class HomeDbContextFactory {
    private readonly IDbContextFactory<HomeDbContext> _inner;
    public HomeDbContextFactory(IDbContextFactory<HomeDbContext> inner) => _inner = inner;
    public HomeDbContext Get() => _inner.CreateDbContext();
    public Task<HomeDbContext> GetAsync(CancellationToken ct = default) => _inner.CreateDbContextAsync(ct);
}
