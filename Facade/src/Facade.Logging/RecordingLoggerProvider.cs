using Microsoft.Extensions.Logging;

namespace dev.kaldiroglu.Facade.Logging;

/// <summary>
/// A custom <see cref="ILoggerProvider"/> — the pluggable backend behind the
/// <see cref="ILogger"/> facade. Registering it with an <see cref="ILoggerFactory"/>
/// swaps the whole logging destination without any client code changing, exactly
/// like binding SLF4J to a different implementation.
/// </summary>
public sealed class RecordingLoggerProvider : ILoggerProvider
{
    private readonly List<LogRecord> _records = new();
    private readonly LogLevel _minLevel;

    public RecordingLoggerProvider(LogLevel minLevel = LogLevel.Information)
    {
        _minLevel = minLevel;
    }

    /// <summary>Everything captured so far, across all categories.</summary>
    public IReadOnlyList<LogRecord> Records => _records;

    public ILogger CreateLogger(string categoryName) => new RecordingLogger(categoryName, _records, _minLevel);

    public void Dispose()
    {
    }
}
