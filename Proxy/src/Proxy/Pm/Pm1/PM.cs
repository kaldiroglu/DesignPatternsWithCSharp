namespace dev.kaldiroglu.Proxy.Pm.Pm1;

/// <summary>
/// Stage 1, no proxy at all. The Prime Minister takes the calls herself. This class does two
/// jobs: it resolves problems, which is her work, and it decides which problems are worth her
/// time, which is not. The screening logic sits inside the class it exists to protect.
/// </summary>
public class PM
{
    public int ProblemsHeard { get; private set; }

    public int ProblemsResolved { get; private set; }

    public void Listen(string problem)
    {
        ProblemsHeard++;
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
        Console.WriteLine($"PM: do not ask me to find a job for you, {name}.");
    }

    /// <summary>A sensible rule in the wrong class: she runs it herself on every call.</summary>
    private static bool WorthMyTime(string problem) =>
        !problem.Contains("job", StringComparison.OrdinalIgnoreCase)
        && !problem.Contains("cousin", StringComparison.OrdinalIgnoreCase);

    private void Resolve(string problem)
    {
        ProblemsResolved++;
        Console.WriteLine("PM: I will have this resolved — " + problem);
    }
}
