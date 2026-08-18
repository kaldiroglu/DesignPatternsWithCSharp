namespace dev.kaldiroglu.Bridge.Basic.Problem;

/// <summary>
/// The other cell: the second refinement, done the same 2nd way — and duplicated for it.
/// </summary>
public class AnotherConcreteImplementation2 : AnotherSubAbstraction
{
    public override void DoIt() =>
        Console.WriteLine("AnotherSubAbstraction, implementation 2: I am doing it!");
}
