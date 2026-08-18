namespace dev.kaldiroglu.Bridge.Basic.Pattern;

/// <summary>
/// A RefinedAbstraction: it holds an implementation and never asks which one.
/// </summary>
public class ASubAbstraction : IAnAbstraction
{
    private readonly IAnAbstractionImplementation _implementation;

    public ASubAbstraction(IAnAbstractionImplementation implementation) =>
        _implementation = implementation ?? throw new ArgumentNullException(nameof(implementation));

    public void DoIt()
    {
        Console.WriteLine("ASubAbstraction: I am the first refinement.");
        _implementation.DoingIt();
    }
}
