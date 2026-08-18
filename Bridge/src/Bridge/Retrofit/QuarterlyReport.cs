namespace dev.kaldiroglu.Bridge.Retrofit;

/// <summary>
/// A RefinedAbstraction. The required interface grows subtypes of its own, which is the half of
/// the argument an Adapter never has.
/// </summary>
public class QuarterlyReport : RegulatoryReport
{
    public QuarterlyReport(IVendorClient engine) : base(engine)
    {
    }

    protected override string StatementFor(string period) =>
        $"select ledger for quarter {period}";
}
