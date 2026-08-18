namespace dev.kaldiroglu.Bridge.Basic.Pattern;

/// <summary>
/// A second RefinedAbstraction — the class that makes the counting argument visible.
/// <para>
/// It cost one class and works with every implementation that exists or ever will. Its
/// counterpart in <c>Bridge.Basic.Problem</c> cost one class <i>per implementation</i>.
/// </para>
/// </summary>
public class AnotherSubAbstraction : IAnAbstraction
{
    private readonly IAnAbstractionImplementation _implementation;

    public AnotherSubAbstraction(IAnAbstractionImplementation implementation) =>
        _implementation = implementation ?? throw new ArgumentNullException(nameof(implementation));

    public void DoIt()
    {
        Console.WriteLine("AnotherSubAbstraction: I am the second refinement.");
        _implementation.DoingIt();
        _implementation.DoingIt(); // a refinement may compose the primitive more than once
    }
}
