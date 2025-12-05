using Home.Api.ExtensionMethods;
using Home.Api.Objects.Mqtt;
using Home.Api.Objects.Mqtt.ExtensionMethods;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MQTT.Actions;

namespace Home.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]
public class SensorController : ControllerBase {
    private readonly IMqttSensorActions _plugActions;

    public SensorController(IMqttSensorActions plugActions) {
        _plugActions = plugActions;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SensorStatus>))]
    public async Task<IActionResult> Get() {
        var result = await _plugActions.GetAllAsync();
        return result.ToOk(x => x.Select(y => y.ToApiObject()).ToList());
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SensorStatus))]
    public async Task<IActionResult> Get(string id) {
        var sensorStatusResult = await _plugActions.GetSensorStatusAsync(id);
        return sensorStatusResult.ToOk(x => x?.ToApiObject());
    }
}
