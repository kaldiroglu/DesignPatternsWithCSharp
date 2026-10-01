namespace dev.kaldiroglu.Command.Hw.Kitchen;

/// <summary>
/// The <b>Command</b>: a ticket. The waiter writes it at the table, and the kitchen cooks it
/// whenever the rail reaches it. It is complete when it is written — dish, table, kitchen —
/// which is what lets it wait.
/// </summary>
public sealed class Order
{
    private readonly Kitchen _kitchen;
    private readonly string _dish;

    public Order(Kitchen kitchen, string dish, int table)
    {
        _kitchen = kitchen;
        _dish = dish;
        Table = table;
    }

    public int Table { get; }

    public void Execute() => _kitchen.Cook(_dish, Table);

    public override string ToString() => _dish + " for table " + Table;
}
