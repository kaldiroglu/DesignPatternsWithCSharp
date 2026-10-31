namespace dev.kaldiroglu.Proxy.Pm.Pm2;

/// <summary>Stage 2: the screening is in the right place, and the citizen had to be rewritten.</summary>
public static class Pm2Demo
{
    public static void Run()
    {
        var pm = new PM();
        var proxy = new PmProxy(pm);
        var ayse = new Citizen("Ayse", proxy);
        var bora = new Citizen("Bora", proxy);

        ayse.TellProblem("the bridge on the coast road is closed");
        Console.WriteLine();
        bora.TellProblem("my cousin needs a job at the ministry");

        Console.WriteLine();
        Console.WriteLine($"calls screened by the proxy : {proxy.CallsScreened}");
        Console.WriteLine($"calls passed to the PM      : {proxy.CallsPassedOn}");
        Console.WriteLine($"problems the PM heard       : {pm.ProblemsHeard}");
        Console.WriteLine("""

            Better: the Prime Minister's class no longer screens anything,
            and one of the two calls never reached her.

            Still wrong: Citizen holds a PmProxy, not a PM. The two classes
            share no type, so the stand-in is visible to every client, and
            every client had to be edited to accept it.

            A stand-in the client can see is not a proxy.
            """);
    }
}
