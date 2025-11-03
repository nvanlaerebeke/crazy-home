using Home.Api.ExtensionMethods;
using Home.Api.Objects.Mqtt;
using Home.Api.Objects.Mqtt.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MQTT.Actions;

namespace Home.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public sealed class DeviceController {
    private readonly IMqttDeviceActions _actions;

    public DeviceController(IMqttDeviceActions actions) {
        _actions = actions;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Device>))]
    public async Task<IActionResult> GetAll() {
        var result = await _actions.GetAllAsync();
        return result.ToOk(x => x.Select(y => y.ToApiObject()));
    }

    [HttpDelete("[action]/{ieeeAddress}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Remove(string ieeeAddress) {
        var result = await _actions.RemoveAsync(ieeeAddress);
        return result.ToOk(_ => new EmptyResult());
    }

    [HttpPut("[action]/{ieeeAddress}/{friendlyName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SetFriendlyName(string ieeeAddress, string friendlyName) {
        var result = await _actions.SetFriendlyNameAsync(ieeeAddress, friendlyName);
        return result.ToOk(_ => new EmptyResult());
    }
}
