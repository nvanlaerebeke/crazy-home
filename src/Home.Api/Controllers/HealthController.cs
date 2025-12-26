using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Home.Api.Controllers;

/// <summary>
///     Controller that exposes the health endpoints
/// </summary>
[AllowAnonymous]
public sealed class HealthController : ControllerBase {
    /// <summary>
    ///     Health endpoint
    /// </summary>
    /// <returns></returns>
    [HttpGet("/livez")]
    [HttpGet("/readyz")]
    [HttpGet("/healthz")]
    public IActionResult Index() {
        return Ok();
    }
}
