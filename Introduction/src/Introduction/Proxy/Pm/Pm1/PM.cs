namespace dev.kaldiroglu.Introduction.Proxy.Pm.Pm1;

/// <summary>
/// Stage 1 — the Prime Minister, who hears every call and screens it herself.
/// </summary>
public class PM
{
    private int problemsHeard;
    private int problemsResolved;

    public void Listen(string problem)
    {
        problemsHeard++;
        Console.WriteLine("PM: listening to you.");
        if (WorthMyTime(problem))
        {
            Resolve(problem);
        }
        else
        {
            Console.WriteLine("PM: that is not something I deal with.");
        }
    }

    public void FindJob(string name)
    {
        Console.WriteLine("PM: do not ask me to find a job for you, " + name + ".");
    }

    /// <summary>
    /// The screening rule — and the problem with this design.
    /// <para>
    /// It is a perfectly sensible rule. It is simply in the wrong class: the Prime Minister
    /// has to run it herself, on every call, before she can do any of her own work.
    /// </para>
    /// </summary>
    private static bool WorthMyTime(string problem) =>
        !problem.ToLowerInvariant().Contains("job")
        && !problem.ToLowerInvariant().Contains("cousin");

    private void Resolve(string problem)
    {
        problemsResolved++;
        Console.WriteLine("PM: I will have this resolved — " + problem);
    }

    public int ProblemsHeard() => problemsHeard;

    public int ProblemsResolved() => problemsResolved;
}
