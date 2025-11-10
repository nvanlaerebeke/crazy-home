using Home.Config;
using Home.Db.Intercept;
using Home.Db.Model;
using Microsoft.EntityFrameworkCore;

namespace Home.Db.Context;

public class HomeDbContext : DbContext {
    private readonly ISettings _settings;
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Setting> Settings => Set<Setting>();
    public DbSet<Theme> Themes => Set<Theme>();

    public HomeDbContext(ISettings settings) {
        _settings = settings;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        base.OnConfiguring(optionsBuilder);

        if (!optionsBuilder.IsConfigured) {
            optionsBuilder.UseSqlite($"Data Source={_settings.ConfigDirectory}/home.sqlite");
        }

        optionsBuilder.AddInterceptors(new LastUpdatedInterceptor());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Device>(e => {
            e.HasIndex(x => x.IeeeAddress).IsUnique();
            e.HasIndex(x => x.FriendlyName).IsUnique();

            //Make it so that the unique is case-insensitive 
            e.Property(x => x.IeeeAddress).UseCollation("NOCASE");
            e.Property(x => x.FriendlyName).UseCollation("NOCASE");

            e.Property(x => x.DeviceType).HasConversion<string>();
            e.Property(x => x.PowerOnBehavior).HasConversion<string>();
        });

        modelBuilder.Entity<Setting>(e => {
            e.HasIndex(x => x.Key).IsUnique();
            e.Property(x => x.Key).UseCollation("NOCASE");
            e.Property(x => x.Key).HasConversion<string>();
        });

        modelBuilder.Entity<User>(e => {
            e.HasIndex(x => x.UserName).IsUnique();
            e.Property(x => x.UserName).UseCollation("NOCASE");
            e.Property(x => x.UserName).HasConversion<string>();
        });

        modelBuilder.Entity<Theme>(e => {
            e.HasIndex(x => x.Name).IsUnique();
            e.Property(x => x.Name).UseCollation("NOCASE");
        });
    }

    // Handy helper to run recommended PRAGMAs for SQLite on Linux
    public async Task ConfigurePragmasAsync(CancellationToken ct = default) {
        await Database.ExecuteSqlRawAsync("PRAGMA journal_mode=WAL;", ct);
        await Database.ExecuteSqlRawAsync("PRAGMA foreign_keys=ON;", ct);
        await Database.ExecuteSqlRawAsync("PRAGMA busy_timeout=5000;", ct);
    }
}
