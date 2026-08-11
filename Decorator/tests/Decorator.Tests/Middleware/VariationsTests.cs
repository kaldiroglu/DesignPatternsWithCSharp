using dev.kaldiroglu.Decorator.Middleware.Domain;
using dev.kaldiroglu.Decorator.Middleware.Solution.Classic;
using dev.kaldiroglu.Decorator.Middleware.Solution.Fluent;
using dev.kaldiroglu.Decorator.Middleware.Solution.Functional;
using Xunit;

namespace dev.kaldiroglu.Decorator.Tests.Middleware;

/// <summary>
/// Three ways of writing the same pattern. The point of these tests is that the three are
/// indistinguishable from outside: same supplier calls, same log lines, same samples.
/// </summary>
public class VariationsTests
{
    private const string Sku = "SKU-200";
    private static readonly TimeSpan Ttl = TimeSpan.FromSeconds(60);

    private static (int Calls, int LogLines, int Samples) RunClassic()
    {
        var clock = IClock.Manual();
        var supplier = SimulatedRemotePriceFeed.WithDefaults(clock).FailNext(1);
        var log = new CallLog();
        var metrics = new Metrics();

        IPriceFeed feed = new TimingPriceFeed(
            new LoggingPriceFeed(
                new CachingPriceFeed(new RetryingPriceFeed(supplier, 3), clock, Ttl),
                log, "orders"),
            clock, metrics);

        feed.QuoteFor(Sku);
        feed.QuoteFor(Sku);
        return (supplier.CallCount, log.Size, metrics.Size);
    }

    private static (int Calls, int LogLines, int Samples) RunFunctional()
    {
        var clock = IClock.Manual();
        var supplier = SimulatedRemotePriceFeed.WithDefaults(clock).FailNext(1);
        var log = new CallLog();
        var metrics = new Metrics();

        var feed = global::dev.kaldiroglu.Decorator.Middleware.Solution.Functional.Middleware.Apply(
            supplier,
            global::dev.kaldiroglu.Decorator.Middleware.Solution.Functional.Middleware.Timing(clock, metrics),
            global::dev.kaldiroglu.Decorator.Middleware.Solution.Functional.Middleware.Logging(log, "orders"),
            global::dev.kaldiroglu.Decorator.Middleware.Solution.Functional.Middleware.Caching(clock, Ttl),
            global::dev.kaldiroglu.Decorator.Middleware.Solution.Functional.Middleware.Retrying(3));

        feed.QuoteFor(Sku);
        feed.QuoteFor(Sku);
        return (supplier.CallCount, log.Size, metrics.Size);
    }

    private static (int Calls, int LogLines, int Samples) RunFluent()
    {
        var clock = IClock.Manual();
        var supplier = SimulatedRemotePriceFeed.WithDefaults(clock).FailNext(1);
        var log = new CallLog();
        var metrics = new Metrics();

        var feed = PriceFeedPipeline.Around(supplier)
            .WithTiming(clock, metrics)
            .WithLogging(log, "orders")
            .WithCache(clock, Ttl)
            .WithRetry(3)
            .Build();

        feed.QuoteFor(Sku);
        feed.QuoteFor(Sku);
        return (supplier.CallCount, log.Size, metrics.Size);
    }

    [Fact(DisplayName = "all three variations produce identical behavior")]
    public void AllThreeAgree()
    {
        var classic = RunClassic();
        var functional = RunFunctional();
        var fluent = RunFluent();

        Assert.Equal(classic, functional);
        Assert.Equal(classic, fluent);
    }

    [Fact(DisplayName = "and the numbers they agree on are the measured ones")]
    public void TheNumbersThemselves()
    {
        var (calls, logLines, samples) = RunClassic();

        Assert.Equal(2, calls);    // one retried failure, then a cache hit
        Assert.Equal(4, logLines); // two requests in, two answers out
        Assert.Equal(2, samples);  // timing is outermost, so it sees both requests
    }

    [Fact(DisplayName = "a one-off concern needs no class at all: a lambda is a feed")]
    public void AnAdHocDecorator()
    {
        var clock = IClock.Manual();
        var supplier = SimulatedRemotePriceFeed.WithDefaults(clock);
        IPriceFeed upperCasing = new global::dev.kaldiroglu.Decorator.Middleware.Solution
            .Functional.Middleware.Lambda(sku => supplier.QuoteFor(sku.ToUpperInvariant()));

        Assert.Equal("SKU-200", upperCasing.QuoteFor("sku-200").Sku);
    }
}
