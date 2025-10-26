using Home.Api.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MQTT.Actions;

namespace Home.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class DeviceController {
    private readonly IMqttDeviceActions _actions;
    public DeviceController(IMqttDeviceActions actions) {
        _actions = actions;
    }
    
    [HttpPut("[action]/{ieeeAddress}/{friendlyName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SetFriendlyName(string ieeeAddress, string friendlyName) {
        var result =await _actions.SetFriendlyNameAsync(ieeeAddress, friendlyName);
        return result.ToOk(_ => new EmptyResult());
    }
}

