namespace dev.kaldiroglu.State.Order.Problem;

/// <summary>
/// Stage one: four flags. An order is paid, shipped and delivered.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/State.Demo -- order-problem-stage-one</c>.
/// </remarks>
public static class StageOneMain
{
    public static void Run()
    {
        var flags = new FlagOrder();
        flags.Pay();
        flags.Ship("TR-1");
        flags.Deliver();
        Console.WriteLine("Stage one, four flags:   " + Show(flags.Events));
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
