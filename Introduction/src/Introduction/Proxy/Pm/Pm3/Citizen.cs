namespace dev.kaldiroglu.Introduction.Proxy.Pm.Pm3;

/// <summary>
/// A citizen who holds an <see cref="IPM"/>.
/// <para>
/// Compare the field with <c>Pm1.Citizen</c> and <c>Pm2.Citizen</c>. In <c>Pm1</c> it was
/// <c>PM</c>, a class; in <c>Pm2</c> it was <c>Proxy</c>. Here it is <see cref="IPM"/>, an
/// interface. The screening now happens, and this class does not know it does.
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
