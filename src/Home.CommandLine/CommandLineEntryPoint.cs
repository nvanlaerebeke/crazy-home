namespace Home.CommandLine;

internal sealed class CommandLineEntryPoint : ICommandLineEntryPoint {
    private bool _configured;
    private readonly RootCommandEntryPoint _entryPoint;
    private static Exception? _exception;

    public CommandLineEntryPoint(RootCommandEntryPoint entryPoint) {
        _entryPoint = entryPoint;
    }

    public async Task StartAsync(string[] args) {
        if (!_configured) {
            _entryPoint.Configure();
            _configured = true;
        }

        var parseResult = _entryPoint.GetCommand().Parse(args);
        await parseResult.InvokeAsync();

        if (_exception is not null) {
            throw _exception;
        }
    }

    public static void SetException(Exception? exception) {
        _exception = exception;
    }
}
