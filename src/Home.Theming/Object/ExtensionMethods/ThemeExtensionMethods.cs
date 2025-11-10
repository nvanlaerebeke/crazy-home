using Home.Db.Model;

namespace Home.Theming.Object.ExtensionMethods;

internal static class ThemeExtensionMethods {
    public static ThemeDto ToDto(this Theme theme) {
        return new() {
            Name = theme.Name, Primary = theme.Primary, Secondary = theme.Secondary, Tertiary = theme.Tertiary
        };
    }
}
