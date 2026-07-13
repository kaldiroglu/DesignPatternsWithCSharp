namespace dev.kaldiroglu.Proxy.Pm.Pm3;

/// <summary>
/// Proxy — implements <see cref="IPM"/> and wraps a real <see cref="IPM"/>. It
/// filters incoming requests and forwards the worthwhile ones to the real
/// subject, which the client never references directly.
/// </summary>
public class ProxyPM : IPM
{
    private readonly IPM _pm;

    public ProxyPM(IPM pm)
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
