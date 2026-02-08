using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTT.Actions;

namespace Home.PowerReader;

internal sealed class PollBackgroundTask : BackgroundService{
    private readonly IMqttPowerActions _actions;
    private readonly IPowerReaderClient _powerReader;
    private readonly ILogger<PollBackgroundTask> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(5);

    public PollBackgroundTask(
        IMqttPowerActions actions,
        IPowerReaderClient powerReader,
        ILogger<PollBackgroundTask> logger
    ) {
        _actions = actions;
        _powerReader = powerReader;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        using var timer = new PeriodicTimer(_checkInterval);

        while (await timer.WaitForNextTickAsync(stoppingToken)) {
            try {
                await SendAsync();
            } catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) {
                /* normal shutdown */
            } catch (Exception ex) {
                _logger.LogError(ex, "Power Reader Polling Failure!");
            }
        }
    }

    private async Task SendAsync() {
        var dataRecord = await _powerReader.GetAsync(CancellationToken.None);
        if (dataRecord is null) {
            return;
        }

        await _actions.PublishPowerUpdateAsync(dataRecord.ToDto());
    }
}
