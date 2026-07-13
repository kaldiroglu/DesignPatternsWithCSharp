namespace dev.kaldiroglu.Proxy.Pm.Pm3;

/// <summary>
/// Stage 3 demo — the citizen asks the secretary for the PM and gets a proxy that
/// implements the same <see cref="IPM"/> interface as the real PM.
/// </summary>
public static class Pm3Demo
{
    public static void Run()
    {
        Console.WriteLine("Everything starts with a citizen coming to the PM Secretary and asking for the PM");
        var secretary = new PMSecretary();
        var citizen = new Citizen("John", secretary);
        citizen.TellProblem();
        citizen.AskForJob();
    }
}
