namespace dev.kaldiroglu.Proxy.Pm.Pm2;

/// <summary>
/// Stage 2 — the real Prime Minister. A <see cref="PmProxy"/> now stands in front
/// of it, filtering requests before delegating. Note the proxy and the PM do not
/// yet share a common interface, so the client is still coupled to concrete types.
/// </summary>
public class PM
{
    public void Listen(string problem)
    {
        Console.WriteLine("PM: Listening to you.");
        Resolve(problem);
    }

    public void FindJob(string name)
    {
        Console.WriteLine("PM: Don't ask me to find a job for you!");
    }

    private static void Resolve(string problem)
    {
        Console.WriteLine($"PM: Please resolve this: {problem}");
    }
}
