using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;

namespace Home.CommandLine.Commands;

internal sealed class Test {
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
            await Task.Delay(5000);
        }
    }

    public static void ConfigureServices(IServiceCollection services) {
    }
}
