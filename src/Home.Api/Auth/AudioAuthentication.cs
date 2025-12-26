using Home.Config;
using Home.Db;
using Home.Db.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace Home.Api.Auth;

public sealed class ApiKeyAuthorizeFilter : IAsyncAuthorizationFilter {
    private const string HeaderName = "X-API-KEY";
    private readonly HomeDbContextFactory _contextFactory;

    public ApiKeyAuthorizeFilter(HomeDbContextFactory context) {
        _contextFactory = context;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context) {
        var request = context.HttpContext.Request;

        if (!request.Headers.TryGetValue(HeaderName, out var providedValues)) {
            context.Result = UnauthorizedProblem($"Missing header '{HeaderName}'.");
            return;
        }

        var provided = providedValues.ToString();
        if (string.IsNullOrWhiteSpace(provided)) {
            context.Result = UnauthorizedProblem($"Empty header '{HeaderName}'.");
            return;
        }

        var expected = await GetExpectedApiKeyAsync();
        if (string.IsNullOrWhiteSpace(expected)) {
            context.Result = UnauthorizedProblem("API key is not configured.");
            return;
        }

        if (expected.Equals(provided, StringComparison.InvariantCulture)) {
            // No context.Result set = Authorized
            return;
        }
        
        context.Result = UnauthorizedProblem("Invalid API key.");
    }

    private async Task<string?> GetExpectedApiKeyAsync() {
        var work = await _contextFactory.GetAsync();
        return (await work.Settings.GetByNameAsync(Spotify.AudioApiKeySettingsName))?.Value;
    }
    
    private static IActionResult UnauthorizedProblem(string detail)
        => new UnauthorizedObjectResult(new ProblemDetails {
            Title = "Unauthorized", Status = StatusCodes.Status401Unauthorized, Detail = detail
        });
}
