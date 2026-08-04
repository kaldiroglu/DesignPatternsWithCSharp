namespace dev.kaldiroglu.Composite.Drawing;

/// <summary>
/// The Component: everything on the canvas can do these, leaf or not.
/// </summary>
/// <remarks>
/// <para>
/// Child management is deliberately <em>not</em> here — see <see cref="ICompositeGraphic"/>.
/// That is GoF's implementation issue 4, and this namespace takes the <b>safe</b> side of it:
/// you cannot call <c>AddGraphic</c> on something that has no children, because the compiler
/// will not let you name the method.
/// </para>
/// <para>
/// The price is that a client wanting to build a tree must hold an
/// <see cref="ICompositeGraphic"/>, not an <c>IGraphic</c>.
/// </para>
/// </remarks>
public interface IGraphic
{
    void Draw();

    void Erase();

    void Paint();

    /// <summary>How many drawable shapes this is, counting recursively. A leaf is one.</summary>
    int ShapeCount();
}
