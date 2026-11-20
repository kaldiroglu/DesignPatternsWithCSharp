using dev.kaldiroglu.State.Order.Problem;

namespace dev.kaldiroglu.State.Order.Solution;

/// <summary>
/// The whole story with both designs: ship, fail three times, ship again, fail three times.
/// It prints what happens to the second shipment, first with stage three's enum, then with
/// State objects.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/State.Demo -- order-side-by-side</c>.
/// </remarks>
public static class SideBySideMain
{
    public static void Run()
    {
        EnumOrder before = new EnumOrder();
        before.Pay();
        before.Ship("TR-1");
        FailThreeTimes(before.FailDelivery);
        before.Ship("TR-2");
        FailThreeTimes(before.FailDelivery);
        Console.WriteLine("Enum constants: " + Show(SecondShipment(before.Events)) + " -> " + before.Status);

        Order after = new Order();
        after.Pay();
        after.Ship("TR-1");
        FailThreeTimes(after.FailDelivery);
        after.Ship("TR-2");
        FailThreeTimes(after.FailDelivery);
        Console.WriteLine("State objects:  " + Show(SecondShipment(after.Events)) + " -> " + after.State);
    }

    private static void FailThreeTimes(Action failDelivery)
    {
        for (int i = 0; i < 3; i++)
        {
            failDelivery();
        }
    }

    private static IEnumerable<string> SecondShipment(IReadOnlyList<string> events) =>
        events.Skip(events.ToList().IndexOf("shipped TR-2"));

    /// <summary>Prints a list as <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
