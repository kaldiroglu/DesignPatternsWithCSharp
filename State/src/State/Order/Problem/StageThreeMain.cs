namespace dev.kaldiroglu.State.Order.Problem;

/// <summary>
/// Stage three: enum constants. After three failed deliveries the parcel goes back to the
/// warehouse.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/State.Demo -- order-problem-stage-three</c>.
/// </remarks>
public static class StageThreeMain
{
    public static void Run()
    {
        var order = SampleOrder.BackFromTheFirstShipment();
        Console.WriteLine("Stage three, first shipment:  " + Show(order.Events) + " -> " + order.Status);
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
