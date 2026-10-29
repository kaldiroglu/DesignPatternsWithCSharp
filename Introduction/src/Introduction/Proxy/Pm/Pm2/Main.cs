namespace dev.kaldiroglu.Introduction.Proxy.Pm.Pm2;

/// <summary>
/// Stage 2 — the screening is in the right place, and the citizen still had to be rewritten.
/// </summary>
/// <remarks>
/// Run it with <c>dotnet run --project src/Introduction.Demo -- pm2</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var pm = new PM();
        var proxy = new Proxy(pm);
        var ayse = new Citizen("Ayse", proxy);
        var bora = new Citizen("Bora", proxy);

        ayse.TellProblem("the bridge on the coast road is closed");
        Console.WriteLine();
        bora.TellProblem("my cousin needs a job at the ministry");

        Console.WriteLine();
        Console.WriteLine($"calls screened by the proxy : {proxy.CallsScreened()}");
        Console.WriteLine($"calls passed to the PM      : {proxy.CallsPassedOn()}");
        Console.WriteLine($"problems the PM heard       : {pm.ProblemsHeard()}");
        Console.WriteLine("""

            Better: the Prime Minister's class no longer screens anything,
            and one of the two calls never reached her.

            Still wrong: Citizen holds a Proxy, not a PM. The two classes
            share no type, so the stand-in is visible to every client — and
            every client had to be edited to accept it.

            A stand-in the client can see is not a proxy.
            """);
    }
}
