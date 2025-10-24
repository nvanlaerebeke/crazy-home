using Microsoft.Extensions.Logging;

namespace Home.CommandLine;

public class NoCategoryLogger : ILogger {
    private readonly TextWriter _writer = Console.Out;

    public IDisposable BeginScope<TState>(TState state) where TState : notnull => NullScope.Instance;
    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter) {
        ArgumentNullException.ThrowIfNull(formatter);

        var message = formatter(state, exception);

        if (!string.IsNullOrEmpty(message)) {
            _writer.WriteLine(message);
        }

        if (exception != null) {
            _writer.WriteLine(exception); // optional: comment this if you don't want exception info
        }
    }

    private class NullScope : IDisposable {
        public static readonly NullScope Instance = new();
        public void Dispose() { }
    }
}
