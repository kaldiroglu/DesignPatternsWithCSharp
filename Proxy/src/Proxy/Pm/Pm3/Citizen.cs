namespace dev.kaldiroglu.Proxy.Pm.Pm3;

/// <summary>
/// A citizen who holds an <see cref="IPM"/>, which is an interface. The screening happens, and
/// this class does not know it does.
/// </summary>
public class Citizen
{
    private readonly string _name;
    private readonly IPM _pm;

    public Citizen(string name, PMSecretary secretary)
    {
        _name = name;
        _pm = secretary.GetPM();
    }

    public void TellProblem(string problem) => _pm.Listen(problem);

    public void AskForJob() => _pm.FindJob(_name);
}
