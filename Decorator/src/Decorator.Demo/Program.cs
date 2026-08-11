using dev.kaldiroglu.Decorator.Middleware.Domain;
using dev.kaldiroglu.Decorator.Middleware.Problem;
using dev.kaldiroglu.Decorator.Middleware.Solution.Classic;
using dev.kaldiroglu.Decorator.Middleware.Solution.Fluent;
using Functional = dev.kaldiroglu.Decorator.Middleware.Solution.Functional.Middleware;

// Problem and Solution.Classic each define a LoggingPriceFeed and a RetryingPriceFeed —
// deliberately, since the deck contrasts them. Java's demo disambiguated with a
// fully-qualified name; C# needs an alias, because `using` imports carry no precedence.
using LoggingPriceFeed = dev.kaldiroglu.Decorator.Middleware.Solution.Classic.LoggingPriceFeed;
using RetryingPriceFeed = dev.kaldiroglu.Decorator.Middleware.Solution.Classic.RetryingPriceFeed;

namespace dev.kaldiroglu.Decorator.Demo;

/// <summary>
/// Runs every design against the same scenario, printing the number that matters each
/// time: how many times the supplier was actually called.
/// <para>
/// Each example runs on its own — <c>dotnet run -- copypaste</c> — so none of them needs a
/// line commented out to be seen alone. With no argument, all of them run in the order the
/// deck presents them.
/// </para>
/// </summary>
public static class Program
{
    private const string Sku = "SKU-200";
    private static readonly TimeSpan Ttl = TimeSpan.FromSeconds(60);

    private static readonly Dictionary<string, (string Group, Action Run)> Examples = new()
    {
        ["copypaste"] = ("THE PROBLEM", CopyPaste),
        ["flags"] = ("THE PROBLEM", Flags),
        ["subclasses"] = ("THE PROBLEM", Subclasses),
        ["classic"] = ("THE SOLUTION — classic decorators", Classic),
        ["ordering"] = ("THE SOLUTION — classic decorators", Ordering),
        ["ratelimit"] = ("THE SOLUTION — classic decorators", RateLimitPlacement),
        ["vendor"] = ("THE SOLUTION — classic decorators", VendorFeed),
        ["functional"] = ("THE SOLUTION — variation: functional", FunctionalVariation),
        ["fluent"] = ("THE SOLUTION — variation: fluent assembly", FluentVariation)
    };

    public static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            var name = args[0].ToLowerInvariant();
            if (!Examples.TryGetValue(name, out var example))
            {
                Console.WriteLine($"unknown example '{name}'. One of: {string.Join(", ", Examples.Keys)}");
                return;
            }

            example.Run();
            return;
        }

        string? lastGroup = null;
        foreach (var (_, (group, run)) in Examples)
        {
            if (group != lastGroup)
            {
                Heading(group);
                lastGroup = group;
            }

            run();
        }
    }

    // ---------------------------------------------------------------- problem

    private static void CopyPaste()
    {
        Section("1. cross-cutting code copied to each call site");
        var clock = IClock.Manual();
        var supplier = SimulatedRemotePriceFeed.WithDefaults(clock);
        var log = new CallLog();
        var service = new CopyPasteOrderService(supplier, clock, log);

        supplier.FailNext(1);
        service.PriceForOrder(Sku);
        var afterOrder = log.Size;

        supplier.FailNext(1);
        service.PriceForReorder("SKU-100");

        Console.WriteLine($"  priceForOrder logged   {afterOrder} lines (retry included)");
        Console.WriteLine($"  priceForReorder logged {log.Size - afterOrder} lines — the failure was "
                          + "never logged, because that line was not copied");
        Console.WriteLine("  the two methods now differ in retry count, logging, and whether");
        Console.WriteLine("  their cache works at all — the reorder copy never writes a timestamp");
    }

    private static void Flags()
    {
        Section("2. one class, five boolean flags");
        var clock = IClock.Manual();
        var supplier = SimulatedRemotePriceFeed.WithDefaults(clock);
        IPriceFeed feed = FlaggedPriceFeed.FullyEnabled(supplier, clock, new CallLog(), new Metrics());

        feed.QuoteFor(Sku);
        feed.QuoteFor(Sku);
        Console.WriteLine($"  works: {supplier.CallCount} supplier call for 2 requests");
        Console.WriteLine("  but: 5 booleans = 32 configurations, and the cache/retry order");
        Console.WriteLine("  is welded into one method that every concern has to share");
    }

    private static void Subclasses()
    {
        Section("3. a subclass per combination");
        var clock = IClock.Manual();
        var supplier = SimulatedRemotePriceFeed.WithDefaults(clock);
        var log = new CallLog();
        IPriceFeed feed = new CachingRetryingLoggingPriceFeed(supplier, 3, log, clock, Ttl);

        supplier.FailNext(1);
        feed.QuoteFor(Sku);
        feed.QuoteFor(Sku);

        Console.WriteLine($"  works: {supplier.CallCount} supplier calls (1 failure retried, then cached)");
        Console.WriteLine("  but: the class name has to list its contents AND their order,");
        Console.WriteLine("  and covering 5 concerns in every order would take 325 classes");
    }

    // ---------------------------------------------------------------- solution

    private static void Classic()
    {
        Section("the same behavior, assembled from five independent decorators");
        var clock = IClock.Manual();
        var supplier = SimulatedRemotePriceFeed.WithDefaults(clock);
        var log = new CallLog();
        var metrics = new Metrics();

        IPriceFeed feed =
            new TimingPriceFeed(
                new LoggingPriceFeed(
                    new CachingPriceFeed(
                        new RetryingPriceFeed(supplier, 3),
                        clock, Ttl),
                    log, "orders"),
                clock, metrics);

        supplier.FailNext(1);
        feed.QuoteFor(Sku);
        feed.QuoteFor(Sku);

        Console.WriteLine($"  supplier calls : {supplier.CallCount}  (1 retried failure, then a cache hit)");
        Console.WriteLine($"  log lines      : {log.Size}");
        Console.WriteLine($"  timed calls    : {metrics.Size}, slowest {metrics.Slowest().TotalMilliseconds} ms");
        Console.WriteLine("  classes needed : 5 decorators + 1 base, for every combination and order");
    }

    private static void Ordering()
    {
        Section("order matters: logging inside or outside the retry");

        var clockA = IClock.Manual();
        var supplierA = SimulatedRemotePriceFeed.WithDefaults(clockA).FailNext(1);
        var outsideLog = new CallLog();
        new LoggingPriceFeed(new RetryingPriceFeed(supplierA, 3), outsideLog, "orders").QuoteFor(Sku);

        var clockB = IClock.Manual();
        var supplierB = SimulatedRemotePriceFeed.WithDefaults(clockB).FailNext(1);
        var insideLog = new CallLog();
        new RetryingPriceFeed(new LoggingPriceFeed(supplierB, insideLog, "orders"), 3).QuoteFor(Sku);

        Console.WriteLine($"  Logging(Retrying(feed)) : {outsideLog.Size} log lines — one request, "
                          + "one story; the failure is invisible");
        Console.WriteLine($"  Retrying(Logging(feed)) : {insideLog.Size} log lines — every attempt is "
                          + "logged, including the one that failed");
        Console.WriteLine($"  Both made {supplierA.CallCount} supplier calls. Same classes,");
        Console.WriteLine("  same settings, different observable behavior — decided by parentheses.");

        Section("order matters: timing inside or outside the cache");

        var clockC = IClock.Manual();
        var supplierC = SimulatedRemotePriceFeed.WithDefaults(clockC);
        var outerMetrics = new Metrics();
        IPriceFeed timingOutside = new TimingPriceFeed(
            new CachingPriceFeed(supplierC, clockC, Ttl), clockC, outerMetrics);
        timingOutside.QuoteFor(Sku);
        timingOutside.QuoteFor(Sku);

        var clockD = IClock.Manual();
        var supplierD = SimulatedRemotePriceFeed.WithDefaults(clockD);
        var innerMetrics = new Metrics();
        IPriceFeed timingInside = new CachingPriceFeed(
            new TimingPriceFeed(supplierD, clockD, innerMetrics), clockD, Ttl);
        timingInside.QuoteFor(Sku);
        timingInside.QuoteFor(Sku);

        Console.WriteLine($"  Timing(Caching(feed)) : {outerMetrics.Size} samples — what the caller "
                          + "waited for, cache hits included");
        Console.WriteLine($"  Caching(Timing(feed)) : {innerMetrics.Size} sample  — what the supplier "
                          + "itself cost, hits never reach it");
        Console.WriteLine("  Both are useful numbers. They are different numbers, and the chain picks.");

        Section("and sometimes the order does not matter — check, do not assume");

        var clockE = IClock.Manual();
        var supplierE = SimulatedRemotePriceFeed.WithDefaults(clockE).FailNext(1);
        IPriceFeed cacheOutside = new CachingPriceFeed(new RetryingPriceFeed(supplierE, 3), clockE, Ttl);
        cacheOutside.QuoteFor(Sku);
        cacheOutside.QuoteFor(Sku);

        var clockF = IClock.Manual();
        var supplierF = SimulatedRemotePriceFeed.WithDefaults(clockF).FailNext(1);
        IPriceFeed retryOutside = new RetryingPriceFeed(new CachingPriceFeed(supplierF, clockF, Ttl), 3);
        retryOutside.QuoteFor(Sku);
        retryOutside.QuoteFor(Sku);

        Console.WriteLine($"  Caching(Retrying(feed)) : {supplierE.CallCount} supplier calls");
        Console.WriteLine($"  Retrying(Caching(feed)) : {supplierF.CallCount} supplier calls");
        Console.WriteLine("  Identical — because failures are not cached, so a retry that passes");
        Console.WriteLine("  back through the cache finds nothing there and goes on to the supplier.");
        Console.WriteLine("  Ordering is something you reason about per pair, not a blanket rule.");
    }

    private static void RateLimitPlacement()
    {
        Section("order matters again: does a cache hit spend quota?");
        var clock = IClock.Manual();
        var supplier = SimulatedRemotePriceFeed.WithDefaults(clock);

        var limiterOutside = new RateLimitingPriceFeed(
            new CachingPriceFeed(supplier, clock, Ttl), clock, 10, TimeSpan.FromHours(1));
        limiterOutside.QuoteFor(Sku);
        limiterOutside.QuoteFor(Sku);
        limiterOutside.QuoteFor(Sku);

        var clock2 = IClock.Manual();
        var supplier2 = SimulatedRemotePriceFeed.WithDefaults(clock2);
        var limiterInside = new RateLimitingPriceFeed(supplier2, clock2, 10, TimeSpan.FromHours(1));
        IPriceFeed quotaSpentOnMisses = new CachingPriceFeed(limiterInside, clock2, Ttl);
        quotaSpentOnMisses.QuoteFor(Sku);
        quotaSpentOnMisses.QuoteFor(Sku);
        quotaSpentOnMisses.QuoteFor(Sku);

        Console.WriteLine($"  RateLimit(Cache(feed)) : {limiterOutside.CallsInWindow} of the quota used "
                          + "for 3 requests — cache hits count against it");
        Console.WriteLine($"  Cache(RateLimit(feed)) : {limiterInside.CallsInWindow} of the quota used "
                          + "for 3 requests — only real calls count");
        Console.WriteLine("  The supplier's contract limits calls to the supplier, so the second is right.");
    }

    private static void VendorFeed()
    {
        Section("decorating a class you are not allowed to derive from");
        var vendor = new VendorPriceFeed(); // sealed: deriving from it will not compile
        var clock = IClock.Manual();
        var log = new CallLog();

        IPriceFeed decorated = new CachingPriceFeed(
            new LoggingPriceFeed(vendor, log, "vendor"), clock, Ttl);
        decorated.QuoteFor("SKU-999");
        decorated.QuoteFor("SKU-999");

        Console.WriteLine($"  vendor calls: {vendor.CallCount} for 2 requests, with logging and caching added");
        Console.WriteLine("  subclassing was never an option here — decoration did not need one");
    }

    private static void FunctionalVariation()
    {
        Section("decorators as functions: no class per concern");
        var clock = IClock.Manual();
        var supplier = SimulatedRemotePriceFeed.WithDefaults(clock);
        var log = new CallLog();
        var metrics = new Metrics();

        var feed = Functional.Apply(supplier,
            Functional.Timing(clock, metrics),   // outermost
            Functional.Logging(log, "orders"),
            Functional.Caching(clock, Ttl),
            Functional.Retrying(3));             // innermost

        supplier.FailNext(1);
        feed.QuoteFor(Sku);
        feed.QuoteFor(Sku);

        Console.WriteLine($"  supplier calls : {supplier.CallCount}  (identical to the classic chain)");
        Console.WriteLine("  written as     : four lambdas, listed outermost first");

        // A one-off concern does not even need a factory.
        IPriceFeed withUppercaseSku = new Functional.Lambda(sku => feed.QuoteFor(sku.ToUpperInvariant()));
        Console.WriteLine($"  ad-hoc decorator: {withUppercaseSku.QuoteFor("sku-200")}");
    }

    private static void FluentVariation()
    {
        Section("the same chain, read in the order a request travels");
        var clock = IClock.Manual();
        var supplier = SimulatedRemotePriceFeed.WithDefaults(clock);
        var log = new CallLog();
        var metrics = new Metrics();

        var feed = PriceFeedPipeline.Around(supplier)
            .WithTiming(clock, metrics)
            .WithLogging(log, "orders")
            .WithCache(clock, Ttl)
            .WithRetry(3)
            .Build();

        supplier.FailNext(1);
        feed.QuoteFor(Sku);
        feed.QuoteFor(Sku);

        Console.WriteLine($"  supplier calls : {supplier.CallCount}  (identical again)");
        Console.WriteLine("  reads top-down instead of inside-out; the behavior is unchanged");

        try
        {
            feed.QuoteFor("SKU-NOPE");
        }
        catch (PriceFeedException e)
        {
            Console.WriteLine($"  unknown sku    : {e.Message} — not retried, because it is not retryable");
        }
    }

    // ---------------------------------------------------------------- output

    private static void Heading(string title)
    {
        Console.WriteLine("\n" + new string('=', 72));
        Console.WriteLine(title);
        Console.WriteLine(new string('=', 72));
    }

    private static void Section(string title) =>
        Console.WriteLine($"\n--- {title} {new string('-', Math.Max(0, 68 - title.Length))}");
}
