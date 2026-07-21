using Microsoft.Extensions.Logging;

namespace dev.kaldiroglu.Facade.Logging;

/// <summary>
/// Subsystem backend: an <see cref="ILogger"/> that keeps entries in memory
/// instead of writing them out. Handy for tests and for showing that the client's
/// logging code is unchanged when the backend is swapped. The .NET counterpart of
/// the Java example's <c>RecordingLoggingEngine</c>.
/// </summary>
public sealed class RecordingLogger : ILogger
{
    private readonly string _category;
    private readonly List<LogRecord> _sink;
    private readonly LogLevel _minLevel;

    public RecordingLogger(string category, List<LogRecord> sink, LogLevel minLevel)
    {
        _category = category;
        _sink = sink;
        _minLevel = minLevel;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None && logLevel >= _minLevel;

    public void Log<TState>(
        LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        // formatter renders the structured message ("Loaded {RowCount} rows ..." -> "Loaded 42 rows ...").
        _sink.Add(new LogRecord(logLevel, _category, formatter(state, exception)));
    }
}
