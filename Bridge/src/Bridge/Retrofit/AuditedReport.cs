namespace dev.kaldiroglu.Bridge.Retrofit;

/// <summary>
/// A second RefinedAbstraction, added because the regulation was amended.
/// <para>
/// It stamps every row, and it names no engine. Written once, it is correct over the legacy
/// engine, over the one bought next year, and over any engine bought after that.
/// </para>
/// </summary>
public class AuditedReport : RegulatoryReport
{
    public AuditedReport(IVendorClient engine) : base(engine)
    {
    }

    protected override string StatementFor(string period) =>
        $"select ledger for quarter {period} with lineage";

    protected override IReadOnlyList<string> DecorateRows(IReadOnlyList<string> rows) =>
        rows.Select(row => $"{row}  [audited]").ToList();
}
