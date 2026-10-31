namespace dev.kaldiroglu.Proxy.Pm.Pm2;

/// <summary>
/// A citizen who now telephones a <see cref="PmProxy"/>. The field type changed from the one in
/// Pm1, so the client was rewritten because the way she is answered changed. That is the
/// defect of this stage.
/// </summary>
public class Citizen
{
    private readonly string _name;
    private readonly PmProxy _proxy;

    public Citizen(string name, PmProxy proxy)
    {
        _name = name;
        _proxy = proxy;
    }

    public void TellProblem(string problem) => _proxy.Listen(problem);

    public void AskForJob() => _proxy.FindJob(_name);
}
