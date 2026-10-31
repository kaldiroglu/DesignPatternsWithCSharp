using Microsoft.Extensions.Logging;

namespace dev.kaldiroglu.Facade.Logging;

/// <summary>
/// Client. It logs through the <see cref="ILogger"/> facade only and never names a provider.
/// The code below stays the same whichever provider is registered behind the facade.
/// </summary>
public sealed class LoggingClient
{
    public static void Run(ILogger log)
    {
        log.LogInformation("Application starting");
        log.LogInformation("Loaded {Rows} rows for user {User}", 42, "john");
        log.LogWarning("Disk usage at {Percent}%", 91);
        log.LogError("Failed to reach {Host} after {Retries} retries", "db-primary", 3);

        // Debug is below the Information threshold. The placeholders already avoid building
        // the final string, but the argument expression would still be evaluated, so a
        // costly argument is guarded with IsEnabled.
        if (log.IsEnabled(LogLevel.Debug))
        {
            log.LogDebug("Full request dump: {Dump}", ExpensiveDump());
        }
    }

    // Stands in for costly work that must not run when the level is disabled.
    private static string ExpensiveDump() => "...lots of expensive-to-build detail...";
}
