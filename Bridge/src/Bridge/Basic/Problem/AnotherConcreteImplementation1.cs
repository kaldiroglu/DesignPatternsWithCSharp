namespace dev.kaldiroglu.Bridge.Basic.Problem;

/// <summary>
/// The other cell: the second refinement, done the same 1st way — and duplicated for it.
/// </summary>
public class AnotherConcreteImplementation1 : AnotherSubAbstraction
{
    public override void DoIt() =>
        Console.WriteLine("AnotherSubAbstraction, implementation 1: I am doing it!");
}
