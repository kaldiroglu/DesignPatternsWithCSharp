namespace dev.kaldiroglu.Proxy.Pm.Pm1;

/// <summary>Stage 1 demo — the citizen reaches the PM directly, no proxy in between.</summary>
public static class Pm1Demo
{
    public static void Run()
    {
        var pm = new PM();
        var citizen = new Citizen("John", pm);
        citizen.TellProblem();
        citizen.AskForJob();
    }
}
