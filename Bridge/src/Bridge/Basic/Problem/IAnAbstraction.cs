namespace dev.kaldiroglu.Bridge.Basic.Problem;

/// <summary>
/// The Abstraction's interface — and the last thing in this namespace that is not a product.
/// <para>
/// Below it, the implementation is chosen by <i>which class you instantiate</i>. So every
/// combination of refinement and implementation needs a class of its own, and the count is
/// m x n rather than m + n.
/// </para>
/// </summary>
public interface IAnAbstraction
{
    void DoIt();
}
