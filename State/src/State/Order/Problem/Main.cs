namespace dev.kaldiroglu.State.Order.Problem;

/// <summary>
/// Runs the three stages. Flags and the switch work; the enum constants work for the first
/// shipment, but the second shipment's failures are counted from 4 and never send it back.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/State.Demo -- order-problem</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var flags = new FlagOrder();
        flags.Pay();
        flags.Ship("TR-1");
        flags.Deliver();
        Console.WriteLine("Stage one, four flags:   " + Show(flags.Events));
        var switching = new SwitchingOrder();
        switching.Pay();
        switching.Ship("TR-1");
        try
        {
            switching.Cancel();
        }
        catch (InvalidOperationException refused)
        {
            Console.WriteLine("Stage two, one switch:   " + refused.Message);
        }
        var order = new EnumOrder();
        order.Pay();
        order.Ship("TR-1");
        for (int i = 0; i < 3; i++)
        {
            order.FailDelivery();
        }
        Console.WriteLine("Stage three, first shipment:  " + Show(order.Events) + " -> " + order.Status);
        order.Ship("TR-2");
        for (int i = 0; i < 3; i++)
        {
            order.FailDelivery();
        }
        var events = order.Events;
        int start = events.ToList().IndexOf("shipped TR-2");
        Console.WriteLine("Stage three, second shipment: " + Show(events.Skip(start)) + " -> " + order.Status);
        Console.WriteLine("The count was not reset, so it never equals 3 again: the courier has no limit.");
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
