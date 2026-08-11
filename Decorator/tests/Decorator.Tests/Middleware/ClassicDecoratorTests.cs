using System.Globalization;
using dev.kaldiroglu.Decorator.Middleware.Domain;
using dev.kaldiroglu.Decorator.Middleware.Solution.Classic;
using Xunit;

namespace dev.kaldiroglu.Decorator.Tests.Middleware;

/// <summary>
/// Each decorator, on its own. Being testable alone is the practical payoff of the
/// pattern: none of these could be written against a flagged god-class, because there is
/// no way to have only one of its concerns.
/// <para>
/// Every number here is the number the Java port asserts, so the two cannot drift apart.
/// </para>
/// </summary>
public class ClassicDecoratorTests
{
    private const string Sku = "SKU-200";
    private static readonly TimeSpan Ttl = TimeSpan.FromSeconds(60);

    private readonly ManualClock _clock = IClock.Manual();
    private readonly SimulatedRemotePriceFeed _supplier;

    public ClassicDecoratorTests() => _supplier = SimulatedRemotePriceFeed.WithDefaults(_clock);

    [Fact(DisplayName = "logging: records the request and the result, and rethrows failures")]
    public void Logging()
    {
        var log = new CallLog();
        IPriceFeed feed = new LoggingPriceFeed(_supplier, log, "orders");

        Assert.Equal("249.00", feed.QuoteFor(Sku).Amount.ToString("0.00", CultureInfo.InvariantCulture));
        Assert.Equal(2, log.Size);
    }

    [Fact(DisplayName = "retrying: forwards many times, but not for a failure that cannot succeed")]
    public void Retrying()
    {
        IPriceFeed feed = new RetryingPriceFeed(_supplier, 3);

        _supplier.FailNext(2);
        Assert.Equal("249.00", feed.QuoteFor(Sku).Amount.ToString("0.00", CultureInfo.InvariantCulture));
        Assert.Equal(3, _supplier.CallCount); // two outages, then an answer

        _supplier.ResetCallCount();
        Assert.Throws<UnknownSkuException>(() => feed.QuoteFor("SKU-NOPE"));
        Assert.Equal(1, _supplier.CallCount); // not retried: it is not retryable
    }

    [Fact(DisplayName = "caching: forwards zero times on a hit")]
    public void Caching()
    {
        var cache = new CachingPriceFeed(_supplier, _clock, Ttl);

        cache.QuoteFor(Sku);
        cache.QuoteFor(Sku);

        Assert.Equal(1, _supplier.CallCount);
        Assert.Equal(1, cache.Hits);
        Assert.Equal(1, cache.Misses);
    }

    [Fact(DisplayName = "rate limiting: refuses once the quota is spent")]
    public void RateLimiting()
    {
        var limited = new RateLimitingPriceFeed(_supplier, _clock, 2, TimeSpan.FromHours(1));

        limited.QuoteFor(Sku);
        limited.QuoteFor(Sku);

        Assert.Throws<RateLimitExceededException>(() => limited.QuoteFor(Sku));
        Assert.Equal(2, limited.CallsInWindow);
    }

    [Fact(DisplayName = "decoration works on a sealed class, where subclassing cannot")]
    public void DecoratesASealedClass()
    {
        var vendor = new VendorPriceFeed();
        var log = new CallLog();

        IPriceFeed decorated = new CachingPriceFeed(
            new LoggingPriceFeed(vendor, log, "vendor"), _clock, Ttl);
        decorated.QuoteFor("SKU-999");
        decorated.QuoteFor("SKU-999");

        Assert.Equal(1, vendor.CallCount); // the cache absorbed the second request
        Assert.Equal(2, log.Size);         // logging sits outside the cache, so it saw both
    }

    [Fact(DisplayName = "the same chain wraps any supplier, however it gets its prices")]
    public void TheSameChainWrapsAnySupplier()
    {
        var vendor = new VendorPriceFeed();
        var vendorLog = new CallLog();
        IPriceFeed decoratedVendor = new CachingPriceFeed(
            new LoggingPriceFeed(vendor, vendorLog, "vendor"), _clock, Ttl);

        var remoteLog = new CallLog();
        IPriceFeed decoratedRemote = new CachingPriceFeed(
            new LoggingPriceFeed(_supplier, remoteLog, "remote"), _clock, Ttl);

        Assert.Equal("42.00", decoratedVendor.QuoteFor("SKU-999").Amount.ToString("0.00", CultureInfo.InvariantCulture));
        Assert.Equal("42.00", decoratedVendor.QuoteFor("SKU-999").Amount.ToString("0.00", CultureInfo.InvariantCulture));
        Assert.Equal("249.00", decoratedRemote.QuoteFor(Sku).Amount.ToString("0.00", CultureInfo.InvariantCulture));
        Assert.Equal("249.00", decoratedRemote.QuoteFor(Sku).Amount.ToString("0.00", CultureInfo.InvariantCulture));

        Assert.Equal(1, vendor.CallCount);
        Assert.Equal(1, _supplier.CallCount);
        Assert.Equal(2, vendorLog.Size);
        Assert.Equal(2, remoteLog.Size);
    }

    [Fact(DisplayName = "ordering: logging inside or outside the retry gives 2 lines or 4")]
    public void OrderingChangesTheEvidence()
    {
        var clockA = IClock.Manual();
        var supplierA = SimulatedRemotePriceFeed.WithDefaults(clockA).FailNext(1);
        var outsideLog = new CallLog();
        new LoggingPriceFeed(new RetryingPriceFeed(supplierA, 3), outsideLog, "orders")
            .QuoteFor(Sku);

        var clockB = IClock.Manual();
        var supplierB = SimulatedRemotePriceFeed.WithDefaults(clockB).FailNext(1);
        var insideLog = new CallLog();
        new RetryingPriceFeed(new LoggingPriceFeed(supplierB, insideLog, "orders"), 3)
            .QuoteFor(Sku);

        Assert.Equal(2, outsideLog.Size); // one request, one story; the failure is invisible
        Assert.Equal(4, insideLog.Size);  // every attempt logged, the failed one included
        Assert.Equal(2, supplierA.CallCount);
        Assert.Equal(supplierA.CallCount, supplierB.CallCount); // identical supplier traffic
    }

    [Fact(DisplayName = "GoF Consequence 3: the decorated feed is not the same object as the feed")]
    public void IdentityIsNotPreserved()
    {
        IPriceFeed decorated = new RetryingPriceFeed(_supplier, 3);

        Assert.NotSame(_supplier, decorated);
        Assert.IsType<RetryingPriceFeed>(decorated);
        Assert.IsNotType<SimulatedRemotePriceFeed>(decorated);
    }

    [Fact(DisplayName = "a decorator must decorate something")]
    public void NullInnerIsRejected() =>
        Assert.Throws<ArgumentNullException>(() => new RetryingPriceFeed(null!, 3));
}
