using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MQTT.Actions;

namespace Home.CommandLine.Commands;

internal sealed class Test {
    private readonly ILogger<Test> _logger;

    public Test(ILogger<Test> logger) {
        _logger = logger;
    }

    public void Configure(Command command) {
        var subCommand = new Command("Test");
        subCommand.SetAction(async _ => { CommandLineEntryPoint.SetException(await Handle()); });

        command.Add(subCommand);
    }

    /// <summary>
    /// Start the command
    /// </summary>
    /// <returns></returns>
    private async Task<Exception?> Handle() {
        while (true) {
            var client = new MqttClient();
            await client.Connect();
        }

        _logger.LogInformation("Done");
        return null;
    }

    public static void ConfigureServices(IServiceCollection services) {
    }
}
