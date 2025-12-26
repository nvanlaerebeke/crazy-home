using Home.Db.Model;
using Microsoft.EntityFrameworkCore;

namespace Home.Db.Repository;

public static class SettingExtensionMethods {
    public static Task<Setting?> GetByNameAsync(this DbSet<Setting> settings, string key) {
        return settings.SingleOrDefaultAsync(s => s.Key == key);
    }
    
    public static async Task SetByKeyAsync(this DbSet<Setting> settings, string key, string value) {
        // This assumes Setting.Name is unique (it should be).
        var entity = await settings.SingleOrDefaultAsync(s => s.Key == key);
        if (entity is null) {
            entity = new Setting { Key = key, Value = value };
            await settings.AddAsync(entity);
        } else {
            entity.Value = value;
        }
    }
}
