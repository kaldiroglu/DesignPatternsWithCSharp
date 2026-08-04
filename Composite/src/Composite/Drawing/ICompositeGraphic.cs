namespace dev.kaldiroglu.Composite.Drawing;

/// <summary>
/// Child management, kept off <see cref="IGraphic"/> on purpose.
/// </summary>
/// <remarks>
/// <para>
/// GoF, implementation issue 4 (Declaring the child management operations, p. 168): declaring
/// <c>Add</c> and <c>Remove</c> on the Component buys <em>transparency</em> — every element
/// looks alike — at the cost of <em>safety</em>, because adding a child to a leaf is then a
/// call that compiles and fails at run time. Declaring it here instead buys safety and costs
/// transparency.
/// </para>
/// <para>
/// Neither answer is wrong. This namespace takes safety; <c>Composite.Gof</c> takes
/// transparency, so the two can be compared side by side.
/// </para>
/// </remarks>
public interface ICompositeGraphic
{
    void AddGraphic(IGraphic graphic);

    void RemoveGraphic(IGraphic graphic);

    IReadOnlyCollection<IGraphic> Graphics { get; }

    /// <summary>Prints the tree, indenting one level per depth.</summary>
    void ListGraphic();
}
