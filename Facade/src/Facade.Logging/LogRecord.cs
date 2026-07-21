using Microsoft.Extensions.Logging;

namespace dev.kaldiroglu.Facade.Logging;

/// <summary>One captured log entry: its level, category (the logger name), and rendered message.</summary>
public sealed record LogRecord(LogLevel Level, string Category, string Message);
