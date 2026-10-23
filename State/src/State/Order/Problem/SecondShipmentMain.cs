namespace dev.kaldiroglu.State.Order.Problem;

/// <summary>
/// The problem with stage three: the parcel is shipped again and fails three more times. The
/// count was not reset, so the failures are attempts 4, 5 and 6 and the order stays shipped.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/State.Demo -- order-problem-second-shipment</c>.
/// </remarks>
public static class SecondShipmentMain
{
    public static void Run()
    {
        var order = SampleOrder.BackFromTheFirstShipment();
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
