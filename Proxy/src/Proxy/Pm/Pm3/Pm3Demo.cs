namespace dev.kaldiroglu.Proxy.Pm.Pm3;

/// <summary>
/// Stage 3 demo — the citizen asks the secretary for the PM and gets a proxy that
/// implements the same <see cref="IPM"/> interface as the real PM.
/// </summary>
public static class Pm3Demo
{
    /// <summary>The citizen goes to the secretary. No call is made yet.</summary>
    public static void Secretary()
    {
        Console.WriteLine("Everything starts with a citizen coming to the PM Secretary and asking for the PM");
        _ = new Citizen("John", new PMSecretary());
    }

    /// <summary>The citizen's two calls go through the proxy.</summary>
    public static void Screening()
    {
        var citizen = new Citizen("John", new PMSecretary());
        citizen.TellProblem();
        citizen.AskForJob();
    }
}
