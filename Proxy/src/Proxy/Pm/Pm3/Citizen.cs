namespace dev.kaldiroglu.Proxy.Pm.Pm3;

/// <summary>
/// Client that depends only on <see cref="IPM"/>. It obtains its reference from
/// the <see cref="PMSecretary"/> and never knows it is holding a proxy.
/// </summary>
public class Citizen
{
    private readonly string _name;
    private readonly IPM _pm;

    public Citizen(string name, PMSecretary secretary)
    {
        _name = name;
        _pm = secretary.GetMePM();
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
