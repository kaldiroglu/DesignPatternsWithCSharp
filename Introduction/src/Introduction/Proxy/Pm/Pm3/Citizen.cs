namespace dev.kaldiroglu.Introduction.Proxy.Pm.Pm3;

/// <summary>
/// A citizen who holds an <see cref="IPM"/>.
/// <para>
/// Compare the field with <c>Pm2.Citizen</c>: its type is the Prime Minister's again, as it
/// was in <c>Pm1</c>. The screening now happens, and this class does not know it does.
/// </para>
/// </summary>
public class Citizen
{
    private readonly string name;
    private readonly IPM pm;

    public Citizen(string name, PMSecretary secretary)
    {
        this.name = name;
        pm = secretary.GetPM();
    }

    public void TellProblem(string problem) => pm.Listen(problem);

    public void AskForJob() => pm.FindJob(name);
}
