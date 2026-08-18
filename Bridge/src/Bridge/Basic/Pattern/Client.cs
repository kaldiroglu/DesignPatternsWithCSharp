namespace dev.kaldiroglu.Bridge.Basic.Pattern;

/// <summary>
/// The client. It holds the Abstraction and never learns which implementation is behind it.
/// </summary>
public class Client
{
    private readonly IAnAbstraction _anAbstraction;

    public Client(IAnAbstraction anAbstraction) => _anAbstraction = anAbstraction;

    public void Start() => _anAbstraction.DoIt();
}
