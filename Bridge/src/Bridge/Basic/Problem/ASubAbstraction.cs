namespace dev.kaldiroglu.Bridge.Basic.Problem;

/// <summary>
/// A refinement. It cannot do anything on its own — the implementation is a subclass.
/// </summary>
public class ASubAbstraction : IAnAbstraction
{
    public virtual void DoIt() =>
        Console.WriteLine("ASubAbstraction: I am the first refinement, and I do nothing.");
}
