namespace dev.kaldiroglu.Introduction.Proxy.Pm.Pm3;

/// <summary>
/// The Proxy — a protection proxy in GoF's terms (p. 208): it controls access to the real
/// subject.
/// <para>
/// It implements <see cref="IPM"/>, which is what makes it substitutable, and holds an
/// <see cref="IPM"/>, which is what lets it forward. Being both is the solution, and it is the
/// same shape as Decorator — the difference is the intent. A decorator adds to what the
/// subject does; this one decides whether the subject is called at all.
/// </para>
/// <para>
/// Note also that it answers <see cref="FindJob"/> entirely by itself. A proxy is allowed to
/// satisfy a request without ever touching the real subject, which no decorator would do.
/// </para>
/// </summary>
public class ProxyPM : IPM
{
    private readonly IPM pm;
    private int callsScreened;
    private int callsPassedOn;
    private int callsRefused;

    public ProxyPM(IPM pm)
    {
        this.pm = pm ?? throw new ArgumentNullException(nameof(pm),
            "a proxy must stand in front of something");
    }

    public void Listen(string problem)
    {
        callsScreened++;
        Console.WriteLine("ProxyPM: let me hear it first.");
        if (WorthHisTime(problem))
        {
            callsPassedOn++;
            pm.Listen(problem);
        }
        else
        {
            callsRefused++;
            Console.WriteLine("ProxyPM: I am afraid that is not something she deals with.");
        }
    }

    /// <summary>Answered here and never forwarded — the request does not reach the real subject.</summary>
    public void FindJob(string name)
    {
        Console.WriteLine("ProxyPM: I will see what can be done, " + name + ".");
    }

    private static bool WorthHisTime(string problem) =>
        !problem.ToLowerInvariant().Contains("job")
        && !problem.ToLowerInvariant().Contains("cousin");

    public int CallsScreened() => callsScreened;

    public int CallsPassedOn() => callsPassedOn;

    public int CallsRefused() => callsRefused;
}
