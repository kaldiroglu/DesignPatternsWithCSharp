namespace dev.kaldiroglu.Proxy.Pm.Pm1;

/// <summary>Client that depends directly on the concrete <see cref="PM"/>.</summary>
public class Citizen
{
    private readonly string _name;
    private readonly PM _pm;

    public Citizen(string name, PM pm)
    {
        _name = name;
        _pm = pm;
    }

    public void TellProblem()
    {
        _pm.Listen("The problem is ...");
    }

    public void AskForJob()
    {
        _pm.FindJob(_name);
    }
}
