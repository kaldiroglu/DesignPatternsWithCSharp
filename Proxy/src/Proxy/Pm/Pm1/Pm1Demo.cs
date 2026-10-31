namespace dev.kaldiroglu.Proxy.Pm.Pm1;

/// <summary>Stage 1: everyone gets through, and the Prime Minister does her own screening.</summary>
public static class Pm1Demo
{
    public static void Run()
    {
        var pm = new PM();
        var ayse = new Citizen("Ayse", pm);
        var bora = new Citizen("Bora", pm);

        ayse.TellProblem("the bridge on the coast road is closed");
        Console.WriteLine();
        bora.TellProblem("my cousin needs a job at the ministry");
        Console.WriteLine();
        bora.AskForJob();

        Console.WriteLine();
        Console.WriteLine($"problems heard by the PM herself : {pm.ProblemsHeard}");
        Console.WriteLine($"problems actually resolved       : {pm.ProblemsResolved}");
        Console.WriteLine("""

            Both calls reached her. She listened to both, and screened both,
            before doing any work of her own.

            Two things are wrong, and neither is the screening rule:
              1. The Prime Minister's class contains a job that is not hers.
              2. Every citizen holds a direct reference to her. There is no
                 way to change who answers without changing the citizens.
            """);
    }
}
