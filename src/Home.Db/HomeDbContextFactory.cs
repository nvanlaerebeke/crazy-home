using Microsoft.EntityFrameworkCore;

namespace Home.Db;

public sealed class HomeDbContextFactory {
    private readonly IDbContextFactory<Context.HomeDbContext> _inner;
    public HomeDbContextFactory(IDbContextFactory<Context.HomeDbContext> inner) => _inner = inner;
    public Context.HomeDbContext Get() => _inner.CreateDbContext();
    public Task<Context.HomeDbContext> GetAsync(CancellationToken ct = default) => _inner.CreateDbContextAsync(ct);
}
