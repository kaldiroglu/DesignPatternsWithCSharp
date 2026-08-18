using dev.kaldiroglu.Bridge.Retrofit;
using Xunit;

namespace dev.kaldiroglu.Bridge.Tests.Retrofit;

/// <summary>
/// A standard arrives over a system that already works. Nothing is rewritten.
/// <para>
/// These tests assert the three things the slides claim: the engine's own callers are untouched,
/// the required interface works over an engine it was never designed for, and the second engine
/// costs one class and no report.
/// </para>
/// </summary>
public class RetrofitTests
{
    private const string RetrofitNs = "dev.kaldiroglu.Bridge.Retrofit";

    [Fact(DisplayName = "the engine's own callers keep working, untouched")]
    public void LegacyCallersAreUndisturbed()
    {
        var engine = new LegacyEngine();

        // A caller that predates the regulation entirely, calling the engine directly.
        var row = engine.ReportDirectly("select headcount");

        Assert.Contains("SELECT HEADCOUNT", row);
        Assert.Equal(0, engine.OpenSessions);   // it cleaned up after itself, as always
    }

    [Fact(DisplayName = "the required interface works over an engine it was never designed for")]
    public void TheStandardIsSatisfied()
    {
        var engine = new LegacyEngine();
        RegulatoryReport report = new QuarterlyReport(engine);

        var rows = report.Submit("2026-Q1");

        Assert.Single(rows);
        Assert.Contains("QUARTER 2026-Q1", rows[0]);
        Assert.Equal("legacy", report.EngineName);

        // Submit is composed from the engine's own primitives, and released its session.
        Assert.Equal(0, engine.OpenSessions);
        Assert.Equal(["select ledger for quarter 2026-Q1"], engine.StatementsSeen);
    }

    [Fact(DisplayName = "a second engine costs one class, and no report is touched")]
    public void TheSecondEngineChangesNoReport()
    {
        // Written for the legacy engine, and never edited since.
        RegulatoryReport quarterly = new QuarterlyReport(new PurchasedEngine());
        RegulatoryReport audited = new AuditedReport(new PurchasedEngine());

        Assert.Equal("purchased", quarterly.EngineName);
        Assert.Contains("conn[ledger]", quarterly.Submit("2026-Q2")[0]);
        Assert.EndsWith("[audited]", audited.Submit("2026-Q2")[0]);
    }

    [Fact(DisplayName = "the engine can be swapped on a report that already exists")]
    public void TheEngineIsAField()
    {
        RegulatoryReport report = new AuditedReport(new LegacyEngine());
        Assert.Equal("legacy", report.EngineName);

        report.SetEngine(new PurchasedEngine());          // the same report object

        Assert.Equal("purchased", report.EngineName);
        Assert.EndsWith("[audited]", report.Submit("2026-Q3")[0]);
    }

    [Fact(DisplayName = "the implementor does not mirror the abstraction — no report operations on it")]
    public void TheTwoInterfacesAreDifferent()
    {
        var vendorMethods = typeof(IVendorClient).GetMethods()
            .Select(m => m.Name.StartsWith("get_") ? m.Name[4..] : m.Name)
            .OrderBy(n => n).ToArray();

        Assert.Equal(["Name", "Open", "Pull", "Release"], vendorMethods);

        // The word the regulation uses appears nowhere on the engine's interface. If it did, the
        // two interfaces would be the same interface twice and there would be no bridge.
        Assert.DoesNotContain("Submit", vendorMethods);
    }

    [Fact(DisplayName = "both sides are hierarchies, which is what makes it a Bridge and not an Adapter")]
    public void BothSidesAreFamilies()
    {
        Assert.True(typeof(RegulatoryReport).IsAbstract);
        Assert.True(typeof(RegulatoryReport).IsAssignableFrom(typeof(QuarterlyReport)));
        Assert.True(typeof(RegulatoryReport).IsAssignableFrom(typeof(AuditedReport)));

        Assert.True(typeof(IVendorClient).IsInterface);
        Assert.True(typeof(IVendorClient).IsAssignableFrom(typeof(LegacyEngine)));
        Assert.True(typeof(IVendorClient).IsAssignableFrom(typeof(PurchasedEngine)));

        // Counted from the namespace rather than asserted as arithmetic: two refinements and two
        // engines, which is 2 + 2 classes where a wrapper per pair would have written 4 and would
        // write 6 the day a third report arrives.
        var reports = TypeCensus.ConcreteImplementationsOf(RetrofitNs, typeof(RegulatoryReport));
        var engines = TypeCensus.ConcreteImplementationsOf(RetrofitNs, typeof(IVendorClient));

        Assert.Equal(2, reports);
        Assert.Equal(2, engines);
        Assert.Equal(4, reports + engines);   // m + n
        Assert.Equal(4, reports * engines);   // m x n — equal at two by two, and never again
    }
}
