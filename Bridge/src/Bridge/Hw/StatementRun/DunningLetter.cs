namespace dev.kaldiroglu.Bridge.Hw.StatementRun;

/// <summary>A RefinedAbstraction: a polite letter about an unpaid invoice.</summary>
public sealed class DunningLetter : Document
{
    private readonly string _customer;
    private readonly string _invoiceNumber;
    private readonly string _amount;
    private readonly int _daysOverdue;

    public DunningLetter(IMedium medium, string customer, string invoiceNumber,
        string amount, int daysOverdue) : base(medium)
    {
        _customer = customer;
        _invoiceNumber = invoiceNumber;
        _amount = amount;
        _daysOverdue = daysOverdue;
    }

    protected override void Body()
    {
        Medium.Heading(1, "Payment reminder");
        Medium.Field("Customer", _customer);
        Medium.Field("Invoice", _invoiceNumber);
        Medium.Field("Days overdue", _daysOverdue.ToString());
        Medium.Total("Amount outstanding", _amount);
    }
}
