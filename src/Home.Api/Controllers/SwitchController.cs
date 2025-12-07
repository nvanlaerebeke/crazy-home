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
public class SwitchController : ControllerBase {
    private readonly IMqttSwitchActions _actions;

    public SwitchController(IMqttSwitchActions actions) {
        _actions = actions;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SwitchStatus>))]
    public async Task<IActionResult> Get() {
        var result = await _actions.GetAllAsync();
        return result.ToOk(x => x.Select(y => y.ToApiObject()).ToList());
    }
    
    [HttpGet("{identifier}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SwitchStatus))]
    public async Task<IActionResult> Get(string identifier) {
        var plugStatusResult = await _actions.GetSwitchInfoAsync(identifier);
        return plugStatusResult.ToOk(x => x?.ToApiObject());
    }

    [HttpPost("{identifier}/[action]/{switchState}")]
    public async Task<IActionResult> State(string identifier, SwitchState switchState) {
        var result = await _actions.SetStateAsync(identifier, switchState);
        return result.ToOk(_ => Ok());
    }
}
