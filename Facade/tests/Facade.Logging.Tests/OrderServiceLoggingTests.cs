using dev.kaldiroglu.Facade.Logging;
using Microsoft.Extensions.Logging;
using Xunit;

namespace dev.kaldiroglu.Facade.Logging.Tests;

/// <summary>
/// Verifies that OrderService logs through the ILogger facade, and that a custom
/// provider (the subsystem) captures those logs unchanged.
/// </summary>
public class OrderServiceLoggingTests
{
    private static (OrderService service, RecordingLoggerProvider backend) Build(LogLevel min = LogLevel.Information)
    {
        var backend = new RecordingLoggerProvider(min);
        ILoggerFactory factory = LoggerFactory.Create(b => b.SetMinimumLevel(min).AddProvider(backend));
        return (new OrderService(factory.CreateLogger<OrderService>()), backend);
    }

    [Fact]
    public void StructuredPlaceholdersAreExpandedByTheFacade()
    {
        var (service, backend) = Build();

        service.ConfirmOrder("user_42", "ORD-9876", rowCount: 42);

        Assert.Contains(backend.Records, r => r.Message == "Loaded 42 rows for user user_42");
        Assert.Contains(backend.Records, r => r.Message == "Order ORD-9876 confirmed");
    }

    [Fact]
    public void CategoryIsTheClientType()
    {
        var (service, backend) = Build();

        service.ConfirmOrder("u", "o", rowCount: 1);

        Assert.All(backend.Records, r => Assert.Contains(nameof(OrderService), r.Category));
    }

    [Fact]
    public void DebugIsSuppressedAtInformationLevel()
    {
        var (service, backend) = Build(LogLevel.Information);

        service.ConfirmOrder("u", "o", rowCount: 1);

        Assert.DoesNotContain(backend.Records, r => r.Level == LogLevel.Debug);
    }

    [Fact]
    public void DebugIsCapturedWhenTheBackendEnablesIt()
    {
        var (service, backend) = Build(LogLevel.Debug);

        service.ConfirmOrder("u", "o", rowCount: 1);

        Assert.Contains(backend.Records, r => r.Level == LogLevel.Debug && r.Message.StartsWith("Full order dump"));
    }

    [Fact]
    public void ZeroRowsEmitsAWarning()
    {
        var (service, backend) = Build();

        service.ConfirmOrder("user_7", "ORD-0001", rowCount: 0);

        Assert.Contains(backend.Records, r => r.Level == LogLevel.Warning && r.Message == "No rows found for user user_7");
    }

    [Fact]
    public void SameClientCodeRunsAgainstAnyBackend()
    {
        // The client is constructed with ILogger only; swapping the provider (backend)
        // changes where logs go, not the client. Here we just prove records land.
        var (service, backend) = Build();

        service.ConfirmOrder("user_42", "ORD-9876", rowCount: 42);

        Assert.NotEmpty(backend.Records);
    }
}
