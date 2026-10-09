using System.Globalization;

namespace dev.kaldiroglu.ChainOfResponsibility.Hw.CashDispenser;

/// <summary>
/// Pays three amounts from slots of 200, 100, 50 and 20. Each slot pays what it can and
/// passes the rest on, so 380 and 260 leave 10 unpaid, although other notes would pay them.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/ChainOfResponsibility.Demo -- hw-cashdispenser</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        NoteSlot machine = new NoteSlot(200);
        machine.Then(new NoteSlot(100)).Then(new NoteSlot(50)).Then(new NoteSlot(20));

        foreach (int amount in new[] { 370, 380, 260 })
        {
            Console.WriteLine(amount.ToString(CultureInfo.InvariantCulture) + ": " + Show(machine.Pay(amount)));
        }
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
