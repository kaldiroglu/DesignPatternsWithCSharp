namespace dev.kaldiroglu.Proxy.Pm.Pm4;

/// <summary>
/// The RealSubject. The class is <c>internal</c>, so code outside this assembly cannot create
/// one or call it. C# has no access level for one namespace. Only <see cref="ProxyPM"/>
/// creates it.
/// </summary>
internal class RealPM : IPM
{
    public RealPM()
    {
        Console.WriteLine("RealPM: the Prime Minister takes office.");
    }

    public void Listen(string problem)
    {
        Console.WriteLine("RealPM: I will have this resolved — " + problem);
    }

    public void FindJob(string name)
    {
        Console.WriteLine($"RealPM: do not ask me to find a job for you, {name}.");
    }
}
