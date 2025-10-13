using Home.Api.ExtensionMethods;
using Home.Api.Objects.Plugwise;
using Home.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Plugwise.Actions;

namespace Home.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PlugController : ControllerBase {
    private readonly IPlugService _plugService;
    private readonly ISettings _settings;
    private readonly ILogger<PlugController> _logger;

    public PlugController(IPlugService plugService, ISettings settings, ILogger<PlugController> logger) {
        _plugService = plugService;
        _settings = settings;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CircleInfo>))]
    public IActionResult Get() {
        var plugs = new List<CircleInfo>();
        var tasks = new List<Task>();
        _settings.Plugs.Select(p => p).ToList().ForEach(plug => {
            tasks.Add(Task.Run(() => {
                var usageResult = _plugService.Usage(plug.Mac);
                var stateResult = _plugService.CircleInfo(plug.Mac);

                if (
                    !usageResult.IsSuccess || 
                    !stateResult.IsSuccess
                ) {
                    _logger.LogError("Unable to fetch usage or state for {Mac}", plug.Mac);
                    if (usageResult.IsFaulted) {
                        _logger.LogError("Error fetching usage: {Error}", usageResult.Match(_ => string.Empty, ex => ex.Message));
                    }
                    if (stateResult.IsFaulted) {
                        _logger.LogError("Error fetching state: {Error}", stateResult.Match(_ => string.Empty, ex => ex.Message));
                    }
                    return;
                }
            
                var usage = usageResult.Match(u => u, ex => throw ex);
                var circleInfo = stateResult.Match(s => s, ex => throw ex);
                if (!circleInfo.IsComplete()) {
                    return;
                }

                plugs.Add(circleInfo.ToApiObject(plug, usage));
            }));
        });
        Task.WaitAll(tasks.ToArray());
        return Ok(plugs);
    }

    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StickStatus))]
    public IActionResult Initialize() {
        return _plugService.Initialize().ToOk(r => r.ToApiObject());
    }

    [HttpGet("/[controller]/{mac}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CircleInfo))]
    public IActionResult Circle(string mac) {
        var plug = _settings.Plugs.FirstOrDefault(p => p.Mac.Equals(mac));
        if (plug is null) {
            return NotFound();
        }
        
        var usageResult = _plugService.Usage(plug.Mac);
        var stateResult = _plugService.CircleInfo(plug.Mac);

        if (
            !usageResult.IsSuccess || 
            !stateResult.IsSuccess
        ) {
            _logger.LogError("Unable to fetch usage or state for {Mac}", plug.Mac);
            if (usageResult.IsFaulted) {
                _logger.LogError("Error fetching usage: {Error}", usageResult.Match(_ => string.Empty, ex => ex.Message));
                return usageResult.ToOk(_ => Ok());
            }
            if (stateResult.IsFaulted) {
                _logger.LogError("Error fetching state: {Error}", stateResult.Match(_ => string.Empty, ex => ex.Message));
                return stateResult.ToOk(_ => Ok());
            }
        }
            
        var usage = usageResult.Match(u => u, ex => throw ex);
        var circleInfo = stateResult.Match(s => s, ex => throw ex);
        return new OkObjectResult(circleInfo.ToApiObject(plug, usage));
    }

    [HttpPost("[action]/{mac}")]
    public IActionResult On(string mac) {
        var plug = _settings.Plugs.FirstOrDefault(p => p.Mac.Equals(mac));
        if(plug is null) {
            return NotFound();
        }
        if (!plug.PowerControl) {
            return Unauthorized();
        }
        return _plugService.On(mac).ToOk(_ => Ok());
    }

    [HttpPost("[action]/{mac}")]
    public IActionResult Off(string mac) {
        var plug = _settings.Plugs.FirstOrDefault(p => p.Mac.Equals(mac));
        if(plug is null) {
            return NotFound();
        }
        if (!plug.PowerControl) {
            return Unauthorized();
        }
        return _plugService.Off(mac).ToOk(_ => Ok());
    }

    [HttpGet("[action]/{mac}")]
    public IActionResult Usage(string mac) {
        if (!_settings.Plugs.Select(p => p.Mac).Contains(mac)) {
            return NotFound();
        }
        return _plugService.Usage(mac).ToOk(r => new Usage(r, "Wh"));
    }

    [HttpGet("[action]/{mac}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Calibration))]
    public IActionResult Calibrate(string mac) {
        var plug = _settings.Plugs.FirstOrDefault(p => p.Mac.Equals(mac));
        if(plug is null) {
            return NotFound();
        }
        if (!plug.PowerControl) {
            return Unauthorized();
        }
        return _plugService.Calibrate(mac).ToOk(r => r.ToApiObject());
    }

    [HttpPost("[action]/{mac}/{unixDStamp}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult SetDateTime(string mac, long unixDStamp) {
        var plug = _settings.Plugs.FirstOrDefault(p => p.Mac.Equals(mac));
        if(plug is null) {
            return NotFound();
        }
        if (!plug.PowerControl) {
            return Unauthorized();
        }
        return _plugService.SetDateTime(mac, unixDStamp).ToOk(_ => Ok());
    }
}
