namespace dev.kaldiroglu.Introduction.Proxy.Pm.Pm3;

/// <summary>
/// The RealSubject: the Prime Minister, doing one job.
/// <para>
/// No screening, no gatekeeping, no awareness that anybody stands in front of her. Compare
/// <c>Pm1.PM</c>, which had to do all three.
/// </para>
/// </summary>
public class RealPM : IPM
{
    private int problemsHeard;

    public void Listen(string problem)
    {
        problemsHeard++;
        Console.WriteLine("RealPM: listening to you.");
        Resolve(problem);
    }

    public void FindJob(string name)
    {
        Console.WriteLine("RealPM: do not ask me to find a job for you, " + name + ".");
    }

    private static void Resolve(string problem)
    {
        Console.WriteLine("RealPM: I will have this resolved — " + problem);
    }

    public int ProblemsHeard() => problemsHeard;
}
