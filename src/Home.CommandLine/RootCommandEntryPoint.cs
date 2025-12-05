using System.CommandLine;
using Home.CommandLine.Commands;

namespace Home.CommandLine;

internal sealed class RootCommandEntryPoint : IRootCommandEntryPoint {
    private readonly Test _test;

    private readonly RootCommand _rootCommand = new();

    public RootCommandEntryPoint(
        Test test
    ) {
        _test = test;
    }

    public void Configure() {
        _test.Configure(_rootCommand);
    }

    public Command GetCommand() {
        return _rootCommand;
    }
}
