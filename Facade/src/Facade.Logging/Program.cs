using dev.kaldiroglu.Facade.Logging;
using Microsoft.Extensions.Logging;

// ILogger is the .NET logging facade — the analog of SLF4J's org.slf4j.Logger.
// The clients (LoggingClient, OrderService) log through ILogger and never name a provider.
// logging-console runs LoggingClient on the console provider; logging-recording runs
// OrderService on a custom in-memory provider.

void ConsoleBackend()
{
    // The client logs through ILogger only. The console provider writes on its own thread,
    // so disposing the factory flushes it before the last line is printed.
    bool debugEnabled;
    using (ILoggerFactory consoleFactory = LoggerFactory.Create(builder => builder
               .SetMinimumLevel(LogLevel.Information)
               .AddSimpleConsole(options => options.SingleLine = true)))
    {
        var log = consoleFactory.CreateLogger<LoggingClient>();
        LoggingClient.Run(log);
        debugEnabled = log.IsEnabled(LogLevel.Debug);
    }

    Console.WriteLine($"IsEnabled(LogLevel.Debug) = {debugEnabled}");
}

void RecordingBackend()
{
    // ── Backend 2: a custom in-memory provider — client code is identical ──
    Console.WriteLine("== Backend 2: custom RecordingLoggerProvider (same client code) ==");
    var recording = new RecordingLoggerProvider(LogLevel.Information);
    using (ILoggerFactory recordingFactory = LoggerFactory.Create(builder => builder
               .SetMinimumLevel(LogLevel.Information)
               .AddProvider(recording)))
    {
        var service = new OrderService(recordingFactory.CreateLogger<OrderService>());
        service.ConfirmOrder("user_42", "ORD-9876", rowCount: 42);
    }

    Console.WriteLine($"Captured {recording.Records.Count} records in memory:");
    foreach (LogRecord r in recording.Records)
    {
        Console.WriteLine($"  [{r.Level}] {r.Category} - {r.Message}");
    }
}

// One slide, one run: `dotnet run -- logging-console` or `-- logging-recording`. With no
// argument, both.
switch (args.FirstOrDefault())
{
    case "logging-console":
        ConsoleBackend();
        break;
    case "logging-recording":
        RecordingBackend();
        break;
    case null:
        ConsoleBackend();
        Console.WriteLine();
        RecordingBackend();
        break;
    default:
        Console.WriteLine($"unknown example '{args[0]}'. One of: logging-console, logging-recording");
        break;
}
