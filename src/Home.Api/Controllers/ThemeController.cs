using System.Globalization;
using Home.Api.ExtensionMethods;
using Home.Api.Objects.Theme;
using Home.Api.Objects.Theme.ExtensionMethods;
using Home.Theming;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Home.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class ThemeController : ControllerBase {
    private readonly IThemeService _themeService;

    public ThemeController(IThemeService themeService) {
        _themeService = themeService;
    }

    [HttpGet("/Theme/Season")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Theme))]
    public async Task<IActionResult> GetSeasonTheme() {
        var result = await _themeService.GetSeasonThemeAsync();
        if (result.IsFaulted) {
            return result.ToOk(x => x?.ToApiObject());
        }

        return result.Match(x => x, _ => null) is null ? NotFound() : result.ToOk(x => x?.ToApiObject());
    }

    [HttpPost("/Theme")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Theme))]
    public async Task<IActionResult> Add([FromBody] Theme theme) {
        var result = await _themeService.AddAsync(theme.ToDto());
        return result.ToOk(x => x.ToApiObject());
    }

    [HttpDelete("/Theme/{name}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(string name) {
        var result = await _themeService.DeleteAsync(name);
        return result.ToOk(_ => new EmptyResult());
    }

    [HttpGet("/Theme/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Theme))]
    public async Task<IActionResult> GetColors(string name) {
        var result = await _themeService.GetColorsAsync(name);
        return result.ToOk(x => x.ToApiObject());
    }

    [HttpGet("/Theme")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<string>))]
    public async Task<IActionResult> GetAll() {
        var result = await _themeService.GetAllAsync();
        return result.ToOk(x => x);
    }

    [HttpPut("/Theme")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Theme))]
    public async Task<IActionResult> Update([FromBody] Theme theme) {
        var result = await _themeService.UpdateAsync(theme.ToDto());
        return result.ToOk(x => x.ToApiObject());
    }

    [HttpPut("/Theme/{name}/Background")]
    [Consumes("multipart/form-data")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [RequestSizeLimit(10 * 1024 * 1024)]
    public async Task<IActionResult> UpdateBackground(string name, IFormFile file) {
        if (file.Length == 0) {
            return BadRequest("No file uploaded.");
        }

        // Optionally validate type/size
        var allowed = new[] { "image/png" };
        if (!allowed.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase)) {
            return BadRequest("Unsupported image type.");
        }

        await using var stream = file.OpenReadStream();
        var result = await _themeService.SetBackgroundAsync(name, stream);
        return !result.IsFaulted ? NoContent() : result.ToOk(_ => new EmptyResult());
    }

    [HttpGet("/Theme/{name}/Background")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Background(string name) {
        var getBackgroundResult = await _themeService.GetBackgroundAsync(name);
        if (getBackgroundResult.IsFaulted) {
            return getBackgroundResult.ToOk(_ => new EmptyResult());
        }

        var asset = getBackgroundResult.Match(x => x, ex => throw ex);

        // Conditional GET: ETag
        if (Request.Headers.TryGetValue(HeaderNames.IfNoneMatch, out var inm)) {
            foreach (var tag in inm) {
                if (string.Equals(tag, asset.ETag, StringComparison.Ordinal)) {
                    return StatusCode(StatusCodes.Status304NotModified);
                }
            }
        }

        // Conditional GET: If-Modified-Since
        if (
            asset.LastModified is not null &&
            Request.Headers.TryGetValue(HeaderNames.IfModifiedSince, out var imsRaw) &&
            DateTimeOffset.TryParse(imsRaw, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal,
                out var ims) &&
            asset.LastModified <= ims.AddSeconds(1)
        ) {
            return StatusCode(StatusCodes.Status304NotModified);
        }

        // Set cache headers
        var headers = Response.GetTypedHeaders();
        headers.CacheControl = new CacheControlHeaderValue { Public = true, MaxAge = TimeSpan.FromDays(1) };
        //headers.ETag = new EntityTagHeaderValue(asset.ETag);

        if (asset.LastModified is not null) {
            headers.LastModified = asset.LastModified;
        }

        // Prefer letting ASP.NET Core stream the body so it can set headers correctly.
        // Range support works only with seekable streams.
        var fileResult = File(asset.Content, asset.ContentType, enableRangeProcessing: asset.SupportsRanges);

        // If we know the length and ranges are disabled, ASP.NET may still set Content-Length.
        if (asset.Length is not null && !asset.SupportsRanges) {
            Response.ContentLength = asset.Length;
        }

        return fileResult;
    }
}
