namespace dev.kaldiroglu.Proxy.Pm.Pm2;

/// <summary>
/// Stage 2 proxy. It intercepts the citizen's requests, decides what to pass on
/// (<see cref="SortOut"/>), and only then delegates to the real <see cref="PM"/>.
/// It is still a hand-rolled stand-in: because it shares no interface with
/// <see cref="PM"/>, the <see cref="Citizen"/> must know it is a proxy. Stage 3
/// fixes that with a common interface.
/// </summary>
public class PmProxy
{
    private readonly PM _pm;

    public PmProxy(PM pm)
    {
        _pm = pm;
    }

    public void Listen(string problem)
    {
        Console.WriteLine("Proxy: Listening to you.");
        if (SortOut(problem))
        {
            Delegate(problem);
        }
    }

    public void FindJob(string name)
    {
        Console.WriteLine("Proxy: 'I'll find out what I can do for you!'");
    }

    private void Delegate(string problem)
    {
        _pm.Listen(problem);
    }

    private static bool SortOut(string problem)
    {
        bool b = true;
        // ...
        return b;
    }
}
