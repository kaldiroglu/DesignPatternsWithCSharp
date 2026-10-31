namespace dev.kaldiroglu.Proxy.Pm.Pm2;

/// <summary>
/// Stage 2, the Prime Minister without the screening. This class does one job, and everything
/// that reaches <see cref="Listen"/> is something she should hear.
/// </summary>
public class PM
{
    public int ProblemsHeard { get; private set; }

    public void Listen(string problem)
    {
        ProblemsHeard++;
        Console.WriteLine("PM: listening to you.");
        Resolve(problem);
    }

    public void FindJob(string name)
    {
        Console.WriteLine($"PM: do not ask me to find a job for you, {name}.");
    }

    private static void Resolve(string problem)
    {
        Console.WriteLine("PM: I will have this resolved — " + problem);
    }
}
