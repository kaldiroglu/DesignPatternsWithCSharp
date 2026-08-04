namespace dev.kaldiroglu.Composite.Gof.Graphics;

/// <summary>
/// Component role of the Composite pattern (GoF, "Design Patterns", p. 163).
/// </summary>
/// <remarks>
/// <para>
/// The abstraction for all drawable objects in a graphics editor. Both primitive
/// graphics (<see cref="Line"/>, <see cref="Rectangle"/>, <see cref="Text"/>) and
/// containers of graphics (<see cref="Picture"/>) are <c>Graphic</c>s, so client
/// code can treat a single line and a whole drawing in exactly the same way.
/// </para>
/// <para>
/// <b>Design decision (GoF, "Declaring the child management operations", p. 168):</b>
/// the child operations are declared here, in the Component, rather than only in
/// <see cref="Picture"/>. That favors <em>transparency</em> — clients never need
/// to know whether they hold a leaf or a composite — at the cost of
/// <em>safety</em>: asking a leaf to add a child is meaningless, so the default
/// implementation fails.
/// </para>
/// </remarks>
public abstract class Graphic
{
    /// <summary>Renders this graphic at the given position.</summary>
    public abstract void Draw(Point at);

    // --- Child management: meaningful for composites, an error for leaves ----

    /// <summary>Adds a child graphic.</summary>
    /// <exception cref="NotSupportedException">
    /// Thrown by default, because a primitive graphic has no children.
    /// <see cref="Picture"/> overrides this.
    /// </exception>
    public virtual void Add(Graphic child) =>
        throw new NotSupportedException($"{GetType().Name} is a leaf and cannot contain children");

    /// <summary>Removes a child graphic.</summary>
    /// <exception cref="NotSupportedException">
    /// Thrown by default, for the same reason as <see cref="Add"/>.
    /// </exception>
    public virtual void Remove(Graphic child) =>
        throw new NotSupportedException($"{GetType().Name} is a leaf and cannot contain children");

    /// <summary>Returns the child at <paramref name="index"/>.</summary>
    /// <exception cref="NotSupportedException">
    /// Thrown by default, for the same reason as <see cref="Add"/>.
    /// </exception>
    public virtual Graphic GetChild(int index) =>
        throw new NotSupportedException($"{GetType().Name} is a leaf and has no children");

    /// <summary>
    /// This graphic's children — empty for a leaf.
    /// </summary>
    /// <remarks>
    /// Unlike <see cref="Add"/>, a read-only view of the children is harmless to
    /// answer for a leaf, so it has a sensible default. Recursive traversals in
    /// client code can rely on it without type tests.
    /// </remarks>
    public virtual IReadOnlyList<Graphic> Children => Array.Empty<Graphic>();

    /// <summary>
    /// Whether this graphic can contain children.
    /// </summary>
    /// <remarks>
    /// GoF ("Maximizing the Component interface", p. 167) notes that a client
    /// sometimes genuinely needs to know. Offering this query is cheaper and
    /// safer than letting clients test the concrete type.
    /// </remarks>
    public virtual bool IsComposite => false;
}
