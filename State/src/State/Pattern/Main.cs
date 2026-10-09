namespace dev.kaldiroglu.State.Pattern;

/// <summary>
/// Shows the shape of the earlier TCP example. Its methods are empty, so running them
/// prints nothing; the full version is in <c>State.Gof.Solution</c>.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/State.Demo -- pattern</c>. The interface is
/// <see cref="ITCPState"/> here, so the lines name it with the C# <c>I</c> prefix.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        ITCPConnection connection = new ConcreteTCPConnection();
        connection.Open();
        connection.Acknowledge();
        connection.Close();
        List<ITCPState> states = [new TCPClosed(), new TCPListen(), new TCPEstablished()];
        foreach (ITCPState state in states)
        {
            Console.WriteLine(state.GetType().Name + " implements " + nameof(ITCPState));
        }
        Console.WriteLine("The methods of this version are empty: it shows only the classes of the pattern.");
    }
}
