using Home.Api.ExtensionMethods;
using Home.Api.Objects.Mqtt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MQTT.Actions;

namespace Home.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MqttController : ControllerBase {
    private readonly IMqttSensorActions _plugActions;

    public MqttController(IMqttSensorActions plugActions) {
        _plugActions = plugActions;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SensorStatus>))]
    public async Task<IActionResult> PermitJoining() {
        var result = await _plugActions.PermitJoiningAsync();
        return result.ToOk(_ => Ok());
    }
}
