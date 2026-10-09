namespace dev.kaldiroglu.State.Hw.Vending;

/// <summary>
/// A vending machine with one drink: a drink is sold, the machine is sold out and returns
/// the next coin, and a refill makes it wait for a coin again.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/State.Demo -- hw-vending</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var machine = new VendingMachine(1);
        machine.PressButton();
        machine.InsertCoin();
        machine.InsertCoin();
        machine.PressButton();
        Console.WriteLine("After one sale: " + machine.State);
        machine.InsertCoin();
        machine.Refill(5);
        Console.WriteLine("After a refill: " + machine.State);
        Console.WriteLine("Log: " + Show(machine.Log));
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
