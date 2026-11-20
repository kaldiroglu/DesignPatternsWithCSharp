namespace dev.kaldiroglu.State.Order.Solution;

/// <summary>
/// Prints the order's finite state machine as a table, read from the state classes.
/// <para>
/// A row is a state, a column is an event, and a cell is the state the order is in after
/// that event. A dash means the state refuses the event. Nothing in the table is written by
/// hand: each cell puts a new order into the row's state, sends the column's event, and
/// reads the state that comes back.
/// </para>
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/State.Demo -- order-transition-table</c>.
/// </remarks>
public static class TransitionTableMain
{
    private static readonly string[] States = ["placed", "paid", "shipped", "delivered", "cancelled"];
    private static readonly string[] Events = ["Pay", "Ship", "FailDelivery", "Deliver", "Cancel"];

    public static void Run()
    {
        var table = new List<List<string>> { new[] { "" }.Concat(Events).ToList() };
        foreach (var state in States)
        {
            table.Add(new[] { state }.Concat(Events.Select(e => Cell(state, e))).ToList());
        }
        Print(table);
    }

    /// <summary>
    /// The next state after the event, or "-" when it is refused. When the event keeps the
    /// order in the same state, it is sent again, to find the attempt that moves it on.
    /// </summary>
    private static string Cell(string state, string @event)
    {
        var order = OrderIn(state);
        try
        {
            Send(order, @event);
            if (order.State.Name != state)
            {
                return order.State.Name;
            }
            for (int attempt = 2; attempt <= Shipped.MaxAttempts; attempt++)
            {
                Send(order, @event);
                if (order.State.Name != state)
                {
                    return state + "; " + order.State.Name + " on attempt " + attempt;
                }
            }
            return state;
        }
        catch (InvalidOperationException)
        {
            return "-";
        }
    }

    /// <summary>A new order, moved into the named state through its own events.</summary>
    private static Order OrderIn(string state)
    {
        var order = new Order();
        switch (state)
        {
            case "placed": break;
            case "paid": order.Pay(); break;
            case "shipped": order.Pay(); order.Ship("TR-1"); break;
            case "delivered": order.Pay(); order.Ship("TR-1"); order.Deliver(); break;
            case "cancelled": order.Cancel(); break;
            default: throw new ArgumentException("no state " + state);
        }
        return order;
    }

    private static void Send(Order order, string @event)
    {
        switch (@event)
        {
            case "Pay": order.Pay(); break;
            case "Ship": order.Ship("TR-1"); break;
            case "FailDelivery": order.FailDelivery(); break;
            case "Deliver": order.Deliver(); break;
            case "Cancel": order.Cancel(); break;
            default: throw new ArgumentException("no event " + @event);
        }
    }

    private static void Print(List<List<string>> table)
    {
        int columns = table[0].Count;
        var widths = Enumerable.Range(0, columns).Select(c => table.Max(row => row[c].Length)).ToArray();
        foreach (var row in table)
        {
            var line = string.Concat(row.Select((cell, c) => cell.PadRight(widths[c] + 2)));
            Console.WriteLine(line.TrimEnd());
        }
    }
}
