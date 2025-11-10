using Home.Theming.Object;
using LanguageExt.Common;

namespace Home.Theming;

public interface IThemeService {
    Task<Result<ThemeDto>> AddAsync(ThemeDto theme);
    Task<Result<bool>> DeleteAsync(string name);
    Task<Result<ThemeBackground>> GetBackgroundAsync(string name);
    Task<Result<ThemeDto>> GetColorsAsync(string name);
    Task<Result<List<string>>> GetAllAsync();
    Task<Result<bool>> SetBackgroundAsync(string name, byte[] background);
    Task<Result<ThemeDto>> UpdateAsync(ThemeDto theme);
}
