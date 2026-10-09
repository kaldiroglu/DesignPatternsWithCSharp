namespace dev.kaldiroglu.Command.Hw.Kitchen;

/// <summary>
/// Shows orders waiting on a rail: one can be canceled while it waits, and not after it is
/// cooked.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Command.Demo -- hw-kitchen</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var kitchen = new Kitchen();
        var rail = new OrderRail();
        var soup = new Order(kitchen, "soup", 4);
        var salad = new Order(kitchen, "salad", 2);
        var kebab = new Order(kitchen, "kebab", 4);

        rail.Place(soup);
        rail.Place(salad);
        rail.Place(kebab);
        Console.WriteLine("Orders waiting on the rail: " + rail.Waiting);

        rail.CookNext();
        Console.WriteLine("The kitchen cooked: " + Show(kitchen.Cooked()));

        Console.WriteLine("Cancel the salad, still waiting: " + Show(rail.Cancel(salad)));
        Console.WriteLine("Cancel the soup, already cooked: " + Show(rail.Cancel(soup)));

        rail.CookNext();
        Console.WriteLine("The kitchen cooked: " + Show(kitchen.Cooked()));
        Console.WriteLine("Orders waiting on the rail: " + rail.Waiting);
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";

    /// <summary>Prints a boolean the way Java does: <c>true</c>, not <c>True</c>.</summary>
    private static string Show(bool value) => value ? "true" : "false";
}
