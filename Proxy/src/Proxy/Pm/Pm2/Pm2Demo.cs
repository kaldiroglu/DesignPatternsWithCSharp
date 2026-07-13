namespace dev.kaldiroglu.Proxy.Pm.Pm2;

/// <summary>Stage 2 demo — a proxy now sits between the citizen and the PM.</summary>
public static class Pm2Demo
{
    public static void Run()
    {
        var pm = new PM();
        var proxy = new PmProxy(pm);
        var citizen = new Citizen("John", proxy);
        citizen.TellProblem();
        citizen.AskForJob();
    }
}
