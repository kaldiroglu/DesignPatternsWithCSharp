namespace dev.kaldiroglu.Proxy.Pm.Pm3;

/// <summary>
/// The Proxy, a protection proxy: it controls access to the real subject. It implements
/// <see cref="IPM"/>, so it can be substituted, and it holds an <see cref="IPM"/>, so it can
/// forward. It answers <see cref="FindJob"/> by itself: a proxy may satisfy a request without
/// touching the real subject.
/// </summary>
public class ProxyPM : IPM
{
    private readonly IPM _pm;

    public int CallsScreened { get; private set; }

    public int CallsPassedOn { get; private set; }

    public int CallsRefused { get; private set; }

    public ProxyPM(IPM pm)
    {
        ArgumentNullException.ThrowIfNull(pm);
        _pm = pm;
    }

    public void Listen(string problem)
    {
        CallsScreened++;
        Console.WriteLine("ProxyPM: let me hear it first.");
        if (WorthHerTime(problem))
        {
            CallsPassedOn++;
            _pm.Listen(problem);
        }
        else
        {
            CallsRefused++;
            Console.WriteLine("ProxyPM: I am afraid that is not something she deals with.");
        }
    }

    /// <summary>Answered here and never forwarded.</summary>
    public void FindJob(string name)
    {
        Console.WriteLine($"ProxyPM: I will see what can be done, {name}.");
    }

    private static bool WorthHerTime(string problem) =>
        !problem.Contains("job", StringComparison.OrdinalIgnoreCase)
        && !problem.Contains("cousin", StringComparison.OrdinalIgnoreCase);
}
