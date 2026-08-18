namespace dev.kaldiroglu.Bridge.Retrofit;

/// <summary>
/// The Abstraction: the interface the regulation requires, which we do not get to design.
/// <para>
/// The whole retrofit is the field below. The required shape sits on top; the engine we already
/// have sits behind a reference; and <see cref="Submit"/> is a higher-level operation composed
/// from the engine's primitives — <c>Open</c>, <c>Pull</c>, <c>Release</c> — none of which the
/// regulator has ever heard of.
/// </para>
/// <para>
/// The engine is not rewritten and not recompiled. <see cref="LegacyEngine.ReportDirectly"/>
/// still works, and its callers never learn that any of this happened.
/// </para>
/// </summary>
public abstract class RegulatoryReport
{
    protected IVendorClient Engine;

    protected RegulatoryReport(IVendorClient engine) =>
        Engine = engine ?? throw new ArgumentNullException(nameof(engine));

    /// <summary>Bridge, not Adapter: the engine can be swapped on a report that already exists.</summary>
    public void SetEngine(IVendorClient engine) =>
        Engine = engine ?? throw new ArgumentNullException(nameof(engine));

    /// <summary>The one operation the regulation names. Everything else here is ours.</summary>
    public IReadOnlyList<string> Submit(string period)
    {
        var handle = Engine.Open("ledger");
        try
        {
            return DecorateRows(Engine.Pull(handle, StatementFor(period)));
        }
        finally
        {
            Engine.Release(handle);
        }
    }

    /// <summary>Which engine answered, for the audit trail the regulation also wants.</summary>
    public string EngineName => Engine.Name;

    protected abstract string StatementFor(string period);

    protected virtual IReadOnlyList<string> DecorateRows(IReadOnlyList<string> rows) => rows;
}
