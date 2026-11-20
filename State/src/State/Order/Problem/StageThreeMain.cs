namespace dev.kaldiroglu.State.Order.Problem;

/// <summary>
/// Stage three: enum constants. After three failed deliveries the parcel goes back to the
/// warehouse. Then it is shipped again and fails five times. The count was not reset, so
/// the failures are attempts 4 to 8, and the parcel never goes back.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/State.Demo -- order-problem-stage-three</c>.
/// </remarks>
public static class StageThreeMain
{
    public static void Run()
    {
        var order = SampleOrder.BackFromTheFirstShipment();
        Console.WriteLine("Stage three, first shipment:  " + Show(order.Events) + " -> " + order.Status);

        int start = order.Events.Count;
        order.Ship("TR-2");
        for (int i = 0; i < 5; i++)
        {
            order.FailDelivery();
        }
        Console.WriteLine("Stage three, second shipment: " + Show(order.Events.Skip(start)) + " -> " + order.Status);
        Console.WriteLine("The count was not reset, so it never equals 3 again: the courier has no limit.");
    }

    /// <summary>Prints a list as <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
