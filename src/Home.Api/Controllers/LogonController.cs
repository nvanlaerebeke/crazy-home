using System.ComponentModel;
using System.Security.Claims;
using Home.Api.Controllers.Request;
using Home.Api.ExtensionMethods;
using Home.Api.Objects.Auth;
using Home.Api.Objects.Auth.ExtensionMethods;
using Home.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Home.Api.Controllers;

[DisplayName("Authentication")]
[ApiController]
[Route("Auth")]
public sealed class LogonController : ControllerBase {
    private readonly IAuthActions _actions;

    public LogonController(IAuthActions actions) {
        _actions = actions;
    }

    /// <summary>
    /// Log on with a user
    /// </summary>
    /// <param name="logonRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResult))]
    public async Task<IActionResult> Auth([FromBody] LogonRequest logonRequest) {
        var result = await _actions.LogOn(logonRequest.Username, logonRequest.Password);
        return result.ToOk(x => x.ToApiObject());
    }

    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResult))]
    public async Task<IActionResult> Refresh([FromBody] AuthRefresh authRefresh) {
        var result = await _actions.Refresh(authRefresh.RefreshToken);
        return result.ToOk(x => x.ToApiObject());
    }

    [HttpPost("[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> LogOut() {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (userId is null) {
            return NotFound();
        }

        var result = await _actions.LogOut(userId);
        return result.ToOk(_ => new EmptyResult());
    }

    [HttpPut("{userName}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(string userName, [FromBody] AuthUpdateRequest authUpdateRequest) {
        var result = await _actions.UpdateAsync(userName, authUpdateRequest.ToDto());
        return result.ToOk(_ => new EmptyResult());
    }

    [HttpPost("{userName}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(string userName, [FromBody] AuthUpdateRequest authUpdateRequest) {
        var result = await _actions.CreateAsync(userName, authUpdateRequest.Password);
        return result.ToOk(_ => new EmptyResult());
    }
    
    [HttpDelete("{userName}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(string userName) {
        var result = await _actions.DeleteAsync(userName);
        return result.ToOk(_ => new EmptyResult());
    }


    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [HttpGet]
    public IActionResult Get() {
        return Ok(new User { Username = User.Identity?.Name ?? string.Empty });
    }
}
