namespace dev.kaldiroglu.Proxy.Pm.Pm3;

/// <summary>The RealSubject: the Prime Minister, doing one job. She screens nothing.</summary>
public class RealPM : IPM
{
    public int ProblemsHeard { get; private set; }

    public void Listen(string problem)
    {
        ProblemsHeard++;
        Console.WriteLine("RealPM: listening to you.");
        Resolve(problem);
    }

    public void FindJob(string name)
    {
        Console.WriteLine($"RealPM: do not ask me to find a job for you, {name}.");
    }

    private static void Resolve(string problem)
    {
        Console.WriteLine("RealPM: I will have this resolved — " + problem);
    }
}
