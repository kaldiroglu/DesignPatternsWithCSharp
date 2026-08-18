namespace dev.kaldiroglu.Bridge.Basic.Problem;

/// <summary>
/// A second refinement — which is why this namespace now needs four leaf classes.
/// </summary>
public class AnotherSubAbstraction : IAnAbstraction
{
    public virtual void DoIt() =>
        Console.WriteLine("AnotherSubAbstraction: I am the second refinement, and I do nothing.");
}
