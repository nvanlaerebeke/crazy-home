using Home.Theming.Object;

namespace Home.Api.Objects.Theme.ExtensionMethods;

internal static class ThemeExtensionMethods {
    public static ThemeDto ToDto(this Theme theme) {
        return new() {
            Name = theme.Name, Primary = theme.Primary, Secondary = theme.Secondary, Tertiary = theme.Tertiary
        };
    }

    public static Theme ToApiObject(this ThemeDto theme) {
        return new() {
            Name = theme.Name, Primary = theme.Primary, Secondary = theme.Secondary, Tertiary = theme.Tertiary
        };
    }
}
