namespace dev.kaldiroglu.State.Gof.Problem;

/// <summary>
/// Sends the same requests to a connection that switches on its state in every method:
/// one ignored request, an open, some data, an acknowledgment and a close.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/State.Demo -- gof-problem</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var connection = new TCPConnection();
        connection.Acknowledge();
        connection.ActiveOpen();
        connection.Send("hello");
        connection.Acknowledge();
        connection.Close();
        Console.WriteLine("Log:   " + Show(connection.Log));
        Console.WriteLine("State: " + connection.State);
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
