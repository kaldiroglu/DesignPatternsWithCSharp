namespace dev.kaldiroglu.Proxy.Pm.Pm1;

/// <summary>
/// Stage 1 — no proxy at all. The Prime Minister is a single concrete class and
/// the <see cref="Citizen"/> talks to it directly. This is the starting point we
/// will refactor toward the Proxy pattern in <c>Pm2</c> and <c>Pm3</c>.
/// </summary>
public class PM
{
    public void Listen(string problem)
    {
        Console.WriteLine("PM: Listening to you.");
        if (SortOut(problem))
        {
            Resolve(problem);
        }
    }

    public void FindJob(string name)
    {
        Console.WriteLine("PM: Don't ask me to find a job for you!");
    }

    private static bool SortOut(string problem)
    {
        bool b = true;
        // ...decide whether the problem is worth escalating.
        return b;
    }

    private static void Resolve(string problem)
    {
        Console.WriteLine($"PM: Please resolve this: {problem}");
    }
}
