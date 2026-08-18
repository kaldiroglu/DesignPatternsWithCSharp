namespace dev.kaldiroglu.Bridge.Basic.Pattern;

/// <summary>
/// The Abstraction's interface — the solution reduced to its bones.
/// <para>
/// Two abstractions and two implementations are four classes here. In
/// <c>Bridge.Basic.Problem</c> the same four combinations take six. Add a third
/// implementation and this namespace grows by one; the other grows by two.
/// </para>
/// </summary>
public interface IAnAbstraction
{
    void DoIt();
}
