namespace dev.kaldiroglu.Proxy.Pm.Pm4;

/// <summary>
/// Stage 4. The citizen holds an <see cref="IPM"/> and knows nothing else. The proxy creates
/// the real Prime Minister on the first call it has to forward.
/// </summary>
public static class Pm4Demo
{
    public static void Run()
    {
        var proxy = new ProxyPM();
        var ayse = new Citizen("Ayse", proxy);
        var bora = new Citizen("Bora", proxy);
        Console.WriteLine("Prime Minister created yet? " + proxy.RealPMCreated);
        Console.WriteLine();

        bora.TellProblem("my cousin needs a job at the ministry");
        bora.AskForJob();
        Console.WriteLine("Prime Minister created yet? " + proxy.RealPMCreated);
        Console.WriteLine();

        ayse.TellProblem("the bridge on the coast road is closed");
        Console.WriteLine("Prime Minister created yet? " + proxy.RealPMCreated);
        Console.WriteLine();

        Console.WriteLine($"calls screened : {proxy.CallsScreened}");
        Console.WriteLine($"passed on      : {proxy.CallsPassedOn}");
        Console.WriteLine($"refused        : {proxy.CallsRefused}");
    }
}
