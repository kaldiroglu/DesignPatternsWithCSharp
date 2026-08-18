namespace dev.kaldiroglu.Bridge.Hw.StatementRun;

/// <summary>A RefinedAbstraction: what we are owed, and for what.</summary>
public sealed class Invoice : Document
{
    private readonly string _number;
    private readonly string _customer;
    private readonly IReadOnlyList<string[]> _lines;
    private readonly string _total;

    public Invoice(IMedium medium, string number, string customer,
        IReadOnlyList<string[]> lines, string total) : base(medium)
    {
        _number = number;
        _customer = customer;
        _lines = lines.ToList();
        _total = total;
    }

    protected override void Body()
    {
        Medium.Heading(1, $"Invoice {_number}");
        Medium.Field("Customer", _customer);
        Medium.Heading(2, "Items");
        foreach (var line in _lines)
        {
            Medium.Row(line);
        }

        Medium.Total("Amount due", _total);
    }
}
