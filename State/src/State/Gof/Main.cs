namespace dev.kaldiroglu.State.Gof;

/// <summary>Runs the same requests through both versions and prints both logs.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/State.Demo -- gof</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var before = new Problem.TCPConnection();
        before.Acknowledge();
        before.ActiveOpen();
        before.Send("hello");
        before.Acknowledge();
        before.Close();
        Console.WriteLine("Switch:       " + Show(before.Log) + " -> " + before.State);

        var after = new Solution.TCPConnection();
        after.Acknowledge();
        after.ActiveOpen();
        after.Send("hello");
        after.Acknowledge();
        after.Close();
        Console.WriteLine("State objects: " + Show(after.Log) + " -> " + after.State);
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
