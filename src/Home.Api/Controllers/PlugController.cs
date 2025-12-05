using Home.Api.ExtensionMethods;
using Home.Api.Objects.Mqtt;
using Home.Api.Objects.Mqtt.ExtensionMethods;
using Home.Db;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MQTT.Actions;

namespace Home.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]
public class PlugController : ControllerBase {
    private readonly IMqttPlugActions _plugActions;

    public PlugController(IMqttPlugActions plugActions) {
        _plugActions = plugActions;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PlugStatus>))]
    public async Task<IActionResult> Get() {
        var result = await _plugActions.GetAllAsync();
        return result.ToOk(x => x.Select(y => y.ToApiObject()).ToList());
    }
    
    [HttpGet("{identifier}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlugStatus))]
    public async Task<IActionResult> Get(string identifier) {
        var plugStatusResult = await _plugActions.GetPlugInfoAsync(identifier);
        return plugStatusResult.ToOk(x => x?.ToApiObject());
    }

    [HttpPost("{identifier}/[action]/{switchState}")]
    public async Task<IActionResult> State(string identifier, SwitchState switchState) {
        var result = await _plugActions.SetStateAsync(identifier, switchState);
        return result.ToOk(_ => Ok());
    }
    
    [HttpPut("{identifier}/[action]/{switchState}")]
    public async Task<IActionResult> PowerOnBehavior(string identifier, SwitchState switchState) {
        var result = await _plugActions.SetPowerOnBehavior(identifier, switchState);
        return result.ToOk(_ => Ok());
    }
    
    [HttpPut("{identifier}/[action]/{allowStateChange}")]
    public async Task<IActionResult> AllowStateChange(string identifier, bool allowStateChange) {
        var result = await _plugActions.SetAllowStateChange(identifier, allowStateChange);
        return result.ToOk(_ => Ok());
    }
}
