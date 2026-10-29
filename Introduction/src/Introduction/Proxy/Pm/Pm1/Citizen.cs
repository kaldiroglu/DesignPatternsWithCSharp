namespace dev.kaldiroglu.Introduction.Proxy.Pm.Pm1;

/// <summary>A citizen who holds the Prime Minister's own telephone number.</summary>
public class Citizen
{
    private readonly string name;
    private readonly PM pm;

    public Citizen(string name, PM pm)
    {
        this.name = name;
        this.pm = pm;
    }

    public void TellProblem(string problem) => pm.Listen(problem);

    public void AskForJob() => pm.FindJob(name);
}
