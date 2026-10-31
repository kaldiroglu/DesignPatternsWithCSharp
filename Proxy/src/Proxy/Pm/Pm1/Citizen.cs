namespace dev.kaldiroglu.Proxy.Pm.Pm1;

/// <summary>A citizen who holds the Prime Minister's own telephone number.</summary>
public class Citizen
{
    private readonly string _name;
    private readonly PM _pm;

    public Citizen(string name, PM pm)
    {
        _name = name;
        _pm = pm;
    }

    public void TellProblem(string problem) => _pm.Listen(problem);

    public void AskForJob() => _pm.FindJob(_name);
}
