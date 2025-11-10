using Home.Theming.Actions;
using Home.Theming.Object;
using LanguageExt.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Home.Theming;

internal class ThemeService : IThemeService {
    private readonly IServiceProvider _services;

    public ThemeService(IServiceProvider services) {
        _services = services;
    }

    public async Task<Result<ThemeDto>> AddAsync(ThemeDto theme) {
        try {
            return (await _services.GetRequiredService<Add>().AddAsync(theme));
        } catch (Exception ex) {
            return new Result<ThemeDto>(ex);
        }
    }

    public async Task<Result<bool>> DeleteAsync(string name) {
        try {
            await _services.GetRequiredService<Delete>().DeleteAsync(name);
            return true;
        } catch (Exception ex) {
            return new Result<bool>(ex);
        }
    }

    public async Task<Result<ThemeBackground>> GetBackgroundAsync(string name) {
        try {
            return await _services.GetRequiredService<GetBackground>().GetAsync(name);
        } catch (Exception ex) {
            return new Result<ThemeBackground>(ex);
        }
    }

    public async Task<Result<ThemeDto>> GetColorsAsync(string name) {
        try {
            return await _services.GetRequiredService<GetColors>().GetAsync(name);
        } catch (Exception ex) {
            return new Result<ThemeDto>(ex);
        }
    }

    public async Task<Result<List<string>>> GetAllAsync() {
        try {
            return await _services.GetRequiredService<GetThemes>().GetAsync();
        } catch (Exception ex) {
            return new Result<List<string>>(ex);
        }
    }

    public async Task<Result<bool>> SetBackgroundAsync(string name, Stream backgroundImage) {
        try {
            await _services.GetRequiredService<SetBackground>().SetAsync(name, backgroundImage);
            return true;
        } catch (Exception ex) {
            return new Result<bool>(ex);
        }
    }

    public async Task<Result<ThemeDto>> UpdateAsync(ThemeDto theme) {
        try {
            return await _services.GetRequiredService<Update>().UpdateAsync(theme);
        } catch (Exception ex) {
            return new Result<ThemeDto>(ex);
        }
    }

    public async Task<Result<ThemeDto?>> GetSeasonThemeAsync() {
        try {
            return await _services.GetRequiredService<GetSeasonTheme>().GetAsync();
        } catch (Exception ex) {
            return new Result<ThemeDto?>(ex);
        }
    }
}
