using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Home.Db.Intercept;

internal sealed class LastUpdatedInterceptor : SaveChangesInterceptor {
    private static void Touch(DbContextEventData eventData) {
        var ctx = eventData.Context!;
        foreach (var e in ctx.ChangeTracker.Entries<IHasLastUpdated>()) {
            if (e.State is EntityState.Modified or EntityState.Added) {
                e.Entity.LastUpdated = DateTime.UtcNow;
            }
        }
    }

    public override InterceptionResult<int>
        SavingChanges(DbContextEventData eventData, InterceptionResult<int> result) {
        Touch(eventData);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken ct = default) {
        Touch(eventData);
        return base.SavingChangesAsync(eventData, result, ct);
    }
}
