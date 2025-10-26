using Home.Api.ExtensionMethods;
using Home.Api.Objects.Mqtt;
using Home.Api.Objects.Mqtt.ExtensionMethods;
using Home.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MQTT.Actions;

namespace Home.Api.Controllers;

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

    [HttpPost("[action]/{identifier}")]
    public async Task<IActionResult> On(string identifier) {
        var result = await _plugActions.SetStateAsync(identifier, SwitchState.On);
        return result.ToOk(_ => Ok());
    }

    [HttpPost("[action]/{identifier}")]
    public async Task<IActionResult> Off(string identifier) {
        var result = await _plugActions.SetStateAsync(identifier, SwitchState.Off);
        return result.ToOk(_ => Ok());
    }
}
