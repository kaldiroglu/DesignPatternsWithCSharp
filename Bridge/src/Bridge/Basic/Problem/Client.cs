namespace dev.kaldiroglu.Bridge.Basic.Problem;

/// <summary>
/// The client, identical to the one in <c>Bridge.Basic.Pattern</c>. The client is not what
/// differs between the two designs; the number of classes behind it is.
/// </summary>
public class Client
{
    private readonly IAnAbstraction _anAbstraction;

    public Client(IAnAbstraction anAbstraction) => _anAbstraction = anAbstraction;

    public void Start() => _anAbstraction.DoIt();
}
