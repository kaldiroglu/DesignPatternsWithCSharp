using dev.kaldiroglu.Facade.Logging;
using Microsoft.Extensions.Logging;

// ILogger is the .NET logging facade — the analog of SLF4J's org.slf4j.Logger.
// The application (OrderService) logs through ILogger and never names a provider.
// Below, the SAME OrderService runs against two different backends.

void ConsoleBackend()
{
    // ── Backend 1: the built-in Console provider ──
    Console.WriteLine("== Backend 1: Console provider ==");
    using (ILoggerFactory consoleFactory = LoggerFactory.Create(builder => builder
               .SetMinimumLevel(LogLevel.Information)
               .AddSimpleConsole(options => options.SingleLine = true)))
    {
        var service = new OrderService(consoleFactory.CreateLogger<OrderService>());
        service.ConfirmOrder("user_42", "ORD-9876", rowCount: 42);
        service.ConfirmOrder("user_7", "ORD-0001", rowCount: 0);
    }
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
