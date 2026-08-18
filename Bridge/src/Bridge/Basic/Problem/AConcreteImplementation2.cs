namespace dev.kaldiroglu.Bridge.Basic.Problem;

/// <summary>
/// One cell of the grid: the first refinement, done the 2nd way.
/// <para>
/// Read the base-class clause as the claim it makes: this implementation <i>is a</i>
/// refinement. Change the implementation and you have changed the object's type, which is why
/// nothing here can switch implementation once it exists.
/// </para>
/// </summary>
public class AConcreteImplementation2 : ASubAbstraction
{
    public override void DoIt() =>
        Console.WriteLine("ASubAbstraction, implementation 2: I am doing it!");
}
