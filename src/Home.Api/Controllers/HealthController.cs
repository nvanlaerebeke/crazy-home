using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Home.Api.Controllers;

/// <summary>
///     Controller that exposes the health endpoints
/// </summary>
[AllowAnonymous]
public sealed class HealthController : ControllerBase {
    private readonly ILogger<HealthController> _logger;

    /// <summary>
    /// Constructor for the health endpoints controller class
    /// </summary>
    /// <param name="logger"></param>
    public HealthController(ILogger<HealthController> logger) {
        _logger = logger;
    }

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
