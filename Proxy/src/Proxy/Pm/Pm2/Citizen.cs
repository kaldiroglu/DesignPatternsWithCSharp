namespace dev.kaldiroglu.Proxy.Pm.Pm2;

/// <summary>Client that talks to a <see cref="PmProxy"/> instead of the real PM.</summary>
public class Citizen
{
    private readonly string _name;
    private readonly PmProxy _proxy;

    public Citizen(string name, PmProxy proxy)
    {
        _name = name;
        _proxy = proxy;
    }

    public void TellProblem()
    {
        _proxy.Listen("The problem is ...");
    }

    public void AskForJob()
    {
        _proxy.FindJob(_name);
    }
}
