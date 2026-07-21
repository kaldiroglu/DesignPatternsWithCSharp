using Microsoft.Extensions.Logging;

namespace dev.kaldiroglu.Facade.Logging;

/// <summary>
/// Client. It logs through the <see cref="ILogger{T}"/> <b>facade</b> only, and
/// never references a concrete logging provider (Console, Serilog, NLog, ...).
/// This is the .NET counterpart of coding against SLF4J's <c>org.slf4j.Logger</c>.
/// </summary>
public sealed class OrderService
{
    private readonly ILogger<OrderService> _log;

    public OrderService(ILogger<OrderService> log)
    {
        _log = log;
    }

    public void ConfirmOrder(string userId, string orderId, int rowCount)
    {
        // Structured logging: {Placeholders} are the .NET analog of SLF4J's {}.
        _log.LogInformation("Loaded {RowCount} rows for user {UserId}", rowCount, userId);
        _log.LogInformation("Order {OrderId} confirmed", orderId);

        // Guard an expensive argument so it is only built when Debug is enabled —
        // the same efficiency pattern as SLF4J's isDebugEnabled().
        if (_log.IsEnabled(LogLevel.Debug))
        {
            _log.LogDebug("Full order dump: {Dump}", ExpensiveDump(orderId));
        }

        if (rowCount == 0)
        {
            _log.LogWarning("No rows found for user {UserId}", userId);
        }
    }

    private static string ExpensiveDump(string orderId) => $"...expensive detail for {orderId}...";
}
