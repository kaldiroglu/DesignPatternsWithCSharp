namespace dev.kaldiroglu.Proxy.Pm.Pm4;

/// <summary>
/// The Proxy, of two kinds in one class. A protection proxy: it decides whether a call reaches
/// the real subject. A virtual proxy: it starts with no <see cref="RealPM"/> and creates one the
/// first time a call has to be forwarded. A refused call and <see cref="FindJob"/> never create
/// one.
/// </summary>
public class ProxyPM : IPM
{
    private RealPM? _realPM;

    public bool RealPMCreated => _realPM != null;

    public int CallsScreened { get; private set; }

    public int CallsPassedOn { get; private set; }

    public int CallsRefused { get; private set; }

    public void Listen(string problem)
    {
        CallsScreened++;
        Console.WriteLine("ProxyPM: let me hear it first.");
        if (WorthHerTime(problem))
        {
            CallsPassedOn++;
            GetRealPM().Listen(problem);
        }
        else
        {
            CallsRefused++;
            Console.WriteLine("ProxyPM: I am afraid that is not something she deals with.");
        }
    }

    /// <summary>Answered here and never forwarded, so it never creates the real subject.</summary>
    public void FindJob(string name)
    {
        Console.WriteLine($"ProxyPM: I will see what can be done, {name}.");
    }

    private RealPM GetRealPM() => _realPM ??= new RealPM();

    private static bool WorthHerTime(string problem) =>
        !problem.Contains("job", StringComparison.OrdinalIgnoreCase)
        && !problem.Contains("cousin", StringComparison.OrdinalIgnoreCase);
}
