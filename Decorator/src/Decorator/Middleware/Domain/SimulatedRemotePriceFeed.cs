namespace dev.kaldiroglu.Decorator.Middleware.Domain;

/// <summary>
/// Stands in for the supplier's remote service.
/// <para>
/// It is the measuring instrument of this whole example. It counts the calls it receives,
/// so a design that caches can be shown to make fewer calls and a design that retries can
/// be shown to make more — as numbers, not adjectives.
/// </para>
/// <para>
/// Failures are <i>scripted</i>: <see cref="FailNext"/> queues up outages that the next
/// calls will hit. Nothing is random, so every test and demo produces the same output.
/// </para>
/// </summary>
public sealed class SimulatedRemotePriceFeed : IPriceFeed
{
    private readonly Dictionary<string, string> _catalog = new()
    {
        ["SKU-100"] = "19.90",
        ["SKU-200"] = "249.00",
        ["SKU-300"] = "7.45"
    };

    private readonly ManualClock _clock;
    private readonly TimeSpan _latency;
    private readonly Queue<PriceFeedException> _scriptedFailures = new();

    public SimulatedRemotePriceFeed(ManualClock clock, TimeSpan latency)
    {
        _clock = clock;
        _latency = latency;
    }

    /// <summary>A feed with a 200 ms round trip, which is realistic and inconvenient.</summary>
    public static SimulatedRemotePriceFeed WithDefaults(ManualClock clock) =>
        new(clock, TimeSpan.FromMilliseconds(200));

    /// <summary>Queues <paramref name="count"/> outages for the next calls.</summary>
    public SimulatedRemotePriceFeed FailNext(int count)
    {
        for (var i = 0; i < count; i++)
        {
            _scriptedFailures.Enqueue(new FeedUnavailableException("supplier did not answer"));
        }

        return this;
    }

    public Quote QuoteFor(string sku)
    {
        CallCount++;
        _clock.Advance(_latency); // a remote call costs time, and the clock says so

        if (_scriptedFailures.Count > 0)
        {
            throw _scriptedFailures.Dequeue();
        }

        if (!_catalog.TryGetValue(sku, out var amount))
        {
            throw new UnknownSkuException(sku);
        }

        return Quote.Of(sku, amount);
    }

    /// <summary>How many times the supplier was actually called.</summary>
    public int CallCount { get; private set; }

    public void ResetCallCount() => CallCount = 0;
}
