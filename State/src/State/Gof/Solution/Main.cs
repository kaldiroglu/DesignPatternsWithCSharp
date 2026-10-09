namespace dev.kaldiroglu.State.Gof.Solution;

/// <summary>
/// Sends the same requests as the switch version. Each request goes to the current state
/// object, and the state object chooses the next state.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/State.Demo -- gof-solution</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var connection = new TCPConnection();
        Console.WriteLine("State at the start: " + connection.State);
        connection.Acknowledge();
        connection.ActiveOpen();
        Console.WriteLine("State after ActiveOpen: " + connection.State);
        connection.Send("hello");
        connection.Acknowledge();
        connection.Close();
        Console.WriteLine("Log:   " + Show(connection.Log));
        Console.WriteLine("State: " + connection.State);
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
