using Home.Api.ExtensionMethods;
using Home.Api.Objects.Mqtt.ExtensionMethods;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MQTT.Actions;
using Device = Home.Api.Objects.Mqtt.Device;

namespace Home.Api.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

    [HttpDelete("{ieeeAddress}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Remove(string ieeeAddress) {
        var result = await _actions.RemoveAsync(ieeeAddress);
        return result.ToOk(_ => new EmptyResult());
    }

    [HttpPut("{ieeeAddress}/[action]/{label}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Label(string ieeeAddress, string label) {
        var result = await _actions.SetFriendlyNameAsync(ieeeAddress, label);
        return result.ToOk(_ => new EmptyResult());
    }
}
