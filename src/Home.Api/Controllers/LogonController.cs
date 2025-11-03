using System.Security.Claims;
using Home.Api.ExtensionMethods;
using Home.Api.Objects.Auth;
using Home.Api.Objects.Auth.ExtensionMethods;
using Home.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Home.Api.Controllers;

[ApiController]
public sealed class LogonController : ControllerBase {
    private readonly IAuthActions _actions;

    public LogonController(IAuthActions actions) {
        _actions = actions;
    }

    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResult))]
    public async Task<IActionResult> LogOn([FromBody] LogonInfo logonInfo) {
        var result = await _actions.LogOn(logonInfo.Username, logonInfo.Password);
        return result.ToOk(x => x.ToApiObject());
    }

    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResult))]
    public async Task<IActionResult> Refresh([FromBody] AuthRefresh authRefresh) {
        var result = await _actions.Refresh(authRefresh.RefreshToken);
        return result.ToOk(x => x.ToApiObject());
    }

    [HttpPost("[action]")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> LogOut() {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (userId is null) {
            return NotFound();
        }

        var result = await _actions.LogOut(userId);
        return result.ToOk(_ => new EmptyResult());
    }
}
