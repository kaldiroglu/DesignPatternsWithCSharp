using dev.kaldiroglu.State.Order.Problem;

namespace dev.kaldiroglu.State.Order.Solution;

/// <summary>
/// Ships an order, fails three deliveries, ships it again and fails once more — first with
/// stage three's enum, then with State objects.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/State.Demo -- order</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        EnumOrder before = new EnumOrder();
        before.Pay();
        before.Ship("TR-1");
        before.FailDelivery();
        before.FailDelivery();
        before.FailDelivery();
        before.Ship("TR-2");
        before.FailDelivery();
        Console.WriteLine("Enum constants: " + Show(before.Events));
        Console.WriteLine("  status now: " + before.Status);

        Order after = new Order();
        after.Pay();
        after.Ship("TR-1");
        after.FailDelivery();
        after.FailDelivery();
        after.FailDelivery();
        after.Ship("TR-2");
        after.FailDelivery();
        Console.WriteLine("State objects:  " + Show(after.Events));
        Console.WriteLine("  state now: " + after.State);

        try
        {
            after.Cancel();
        }
        catch (InvalidOperationException refused)
        {
            Console.WriteLine("Cancel a shipped order: " + refused.Message);
        }
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
