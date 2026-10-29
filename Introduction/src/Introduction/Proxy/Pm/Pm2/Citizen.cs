namespace dev.kaldiroglu.Introduction.Proxy.Pm.Pm2;

/// <summary>
/// A citizen who now telephones a <see cref="Proxy"/>.
/// <para>
/// Read the field type. In <c>Pm1</c> it was <c>PM</c>; here it is <c>Proxy</c>. The citizen
/// was rewritten because the way she is answered changed — and that is the whole defect of
/// this stage. Putting a stand-in in front of the PM should be invisible to her.
/// </para>
/// </summary>
public class Citizen
{
    private readonly string name;
    private readonly Proxy proxy;      // <-- was PM in Pm1. The client had to change.

    public Citizen(string name, Proxy proxy)
    {
        this.name = name;
        this.proxy = proxy;
    }

    public void TellProblem(string problem) => proxy.Listen(problem);

    public void AskForJob() => proxy.FindJob(name);
}
