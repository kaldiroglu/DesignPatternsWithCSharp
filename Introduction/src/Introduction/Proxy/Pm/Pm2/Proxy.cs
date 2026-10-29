namespace dev.kaldiroglu.Introduction.Proxy.Pm.Pm2;

/// <summary>
/// Stage 2 — a stand-in that screens, and the reason this stage is <b>not yet</b> the
/// solution.
/// <para>
/// The screening has moved out of <see cref="PM"/>, which was the point, and this class does
/// it well. But look at what it <i>is</i>: a class of its own, related to <c>PM</c> by nothing
/// at all. They share no base type, so nothing in the type system says one can stand in for
/// the other.
/// </para>
/// <para>
/// The consequence is in <see cref="Citizen"/>: its field had to change from <c>PM</c> to
/// <c>Proxy</c>. Every client had to be edited to accept the stand-in — which is exactly what
/// a proxy is supposed to make unnecessary.
/// </para>
/// <para>
/// A stand-in the client can see is not a proxy. It is just a different object. <c>Pm3</c>
/// fixes this with one interface.
/// </para>
/// </summary>
public class Proxy
{
    private readonly PM pm;
    private int callsScreened;
    private int callsPassedOn;

    public Proxy(PM pm)
    {
        this.pm = pm;
    }

    public void Listen(string problem)
    {
        callsScreened++;
        Console.WriteLine("Proxy: let me hear it first.");
        if (WorthHisTime(problem))
        {
            callsPassedOn++;
            pm.Listen(problem);
        }
        else
        {
            Console.WriteLine("Proxy: I am afraid that is not something she deals with.");
        }
    }

    public void FindJob(string name)
    {
        Console.WriteLine("Proxy: I will see what can be done, " + name + ".");
    }

    private static bool WorthHisTime(string problem) =>
        !problem.ToLowerInvariant().Contains("job")
        && !problem.ToLowerInvariant().Contains("cousin");

    public int CallsScreened() => callsScreened;

    public int CallsPassedOn() => callsPassedOn;
}
