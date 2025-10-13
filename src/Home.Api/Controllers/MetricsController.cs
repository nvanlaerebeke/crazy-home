using Home.Api.Objects.Plugwise;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Plugwise.Actions;
using Home.Config;

namespace Home.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MetricsController : ControllerBase {
    private readonly IPlugService _plugService;
    private readonly ISettings _settings;
    private readonly ILogger<MetricsController> _logger;

    public MetricsController(IPlugService plugService, ISettings settings, ILogger<MetricsController> logger) {
        _plugService = plugService;
        _settings = settings;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Metrics))]
    public IActionResult Get() {
        var plugs = new List<PlugMetric>();
        _settings.Plugs.Select(p => p).ToList().ForEach(plug => {
            _ = _plugService.Usage(plug.Mac).Match(
                u => {
                    plugs.Add(new PlugMetric(plug, new Usage(u, "Wh")));
                    return true;
                },
                ex => {
                    _logger.LogError("Unable to fetch usage for {Mac}: {Error}", plug.Mac, ex.Message);
                    return false;
                }
            );
        });
        return Ok(new Metrics(plugs));
    }
}
