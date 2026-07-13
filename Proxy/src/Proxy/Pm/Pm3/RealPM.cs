namespace dev.kaldiroglu.Proxy.Pm.Pm3;

/// <summary>RealSubject — the actual Prime Minister behind the proxy.</summary>
public class RealPM : IPM
{
    public void Listen(string problem)
    {
        Console.WriteLine("RealPM: Listening to you.");
        Resolve(problem);
    }

    public void FindJob(string name)
    {
        Console.WriteLine("RealPM: Don't ask me to find a job for you!");
    }

    private static void Resolve(string problem)
    {
        Console.WriteLine($"RealPM: Please resolve this: {problem}");
    }
}
