namespace dev.kaldiroglu.Proxy.Pm.Pm4;

/// <summary>
/// A citizen who holds an <see cref="IPM"/>. This class names no proxy and no secretary, so it
/// does not change when the object behind the interface does.
/// </summary>
public class Citizen(string name, IPM pm)
{
    public void TellProblem(string problem) => pm.Listen(problem);

    public void AskForJob() => pm.FindJob(name);
}
