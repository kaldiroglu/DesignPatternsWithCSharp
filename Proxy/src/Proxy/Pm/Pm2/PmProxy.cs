namespace dev.kaldiroglu.Proxy.Pm.Pm2;

/// <summary>
/// Stage 2, a stand-in that screens, and not yet the solution. The screening has moved out of
/// <see cref="PM"/>, but this class shares no type with it, so <see cref="Citizen"/> had to
/// change its field from <see cref="PM"/> to <see cref="PmProxy"/>. Pm3 fixes that with one
/// interface.
/// </summary>
public class PmProxy
{
    private readonly PM _pm;

    public int CallsScreened { get; private set; }

    public int CallsPassedOn { get; private set; }

    public PmProxy(PM pm)
    {
        _pm = pm;
    }

    public void Listen(string problem)
    {
        CallsScreened++;
        Console.WriteLine("Proxy: let me hear it first.");
        if (WorthHerTime(problem))
        {
            CallsPassedOn++;
            _pm.Listen(problem);
        }
        else
        {
            Console.WriteLine("Proxy: I am afraid that is not something she deals with.");
        }
    }

    public void FindJob(string name)
    {
        Console.WriteLine($"Proxy: I will see what can be done, {name}.");
    }

    private static bool WorthHerTime(string problem) =>
        !problem.Contains("job", StringComparison.OrdinalIgnoreCase)
        && !problem.Contains("cousin", StringComparison.OrdinalIgnoreCase);
}
