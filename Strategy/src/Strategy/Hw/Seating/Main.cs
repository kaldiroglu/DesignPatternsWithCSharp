namespace dev.kaldiroglu.Strategy.Hw.Seating;

/// <summary>
/// Seats a party of two under three policies, and shows one policy refusing where another
/// succeeds.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Strategy.Demo -- hw-seating</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var cabin = SeatPlan.Empty(3, 4);
        var desk = new BookingDesk(new FirstAvailable());

        ISeatingPolicy[] policies = [new FirstAvailable(), new WindowPreferred(), new KeepTogether()];
        Console.WriteLine("An empty cabin of 3 rows, seats A to D, and a party of two:");
        foreach (var policy in policies)
        {
            desk.SetPolicy(policy);
            Console.WriteLine("  " + desk.PolicyName + ": " + Show(desk.Seat(cabin, 2)));
        }

        var scattered = cabin.WithTaken(["1A", "1B", "1C", "2A", "2B", "2C", "3A", "3B", "3C", "3D"]);
        Console.WriteLine("Free seats left: " + Show(scattered.Free()));
        Console.WriteLine("  FIRST_AVAILABLE: " + Show(new FirstAvailable().Allocate(scattered, 2)));
        Console.WriteLine("  KEEP_TOGETHER: " + Show(new KeepTogether().Allocate(scattered, 2))
            + " (no row has two free seats)");
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
