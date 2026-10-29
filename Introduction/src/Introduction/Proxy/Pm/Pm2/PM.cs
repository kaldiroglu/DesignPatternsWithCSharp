namespace dev.kaldiroglu.Introduction.Proxy.Pm.Pm2;

/// <summary>
/// Stage 2 — the Prime Minister, relieved of the screening.
/// <para>
/// This class is now doing one job, which is an improvement on <c>Pm1</c>. Everything that
/// reaches <c>Listen</c> is something she should be hearing.
/// </para>
/// </summary>
public class PM
{
    private int problemsHeard;

    public void Listen(string problem)
    {
        problemsHeard++;
        Console.WriteLine("PM: listening to you.");
        Resolve(problem);
    }

    public void FindJob(string name)
    {
        Console.WriteLine("PM: do not ask me to find a job for you, " + name + ".");
    }

    private static void Resolve(string problem)
    {
        Console.WriteLine("PM: I will have this resolved — " + problem);
    }

    public int ProblemsHeard() => problemsHeard;
}
