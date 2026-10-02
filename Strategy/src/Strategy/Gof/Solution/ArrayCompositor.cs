namespace dev.kaldiroglu.Strategy.Gof.Solution;

/// <summary>
/// A <b>ConcreteStrategy</b>: GoF's <c>ArrayCompositor</c>, which "breaks the components
/// into lines at regular intervals... This is useful for breaking a collection of icons
/// into rows, for example" (p. 316).
/// <para>
/// This is the one worth stopping on. It ignores the line width entirely — it counts
/// components, not columns — and it is still an <see cref="ICompositor"/>. An interface that
/// had been designed around "fit text to a width" could not have held it, and the fact that
/// it fits is the evidence that the interface describes an <em>algorithm</em> rather than a
/// variation on one.
/// </para>
/// </summary>
public sealed class ArrayCompositor : ICompositor
{
    private readonly int _perLine;

    public ArrayCompositor(int perLine)
    {
        if (perLine < 1)
        {
            throw new ArgumentException("a row needs at least one component", nameof(perLine));
        }
        _perLine = perLine;
    }

    public string Name => "ArrayCompositor(" + _perLine + ")";

    public Layout Compose(IReadOnlyList<Component> components, int lineWidth)
    {
        var lines = new List<IReadOnlyList<Component>>();
        var line = new List<Component>();
        foreach (var component in components)
        {
            line.Add(component);
            if (line.Count == _perLine)
            {
                lines.Add([.. line]);
                line.Clear();
            }
        }
        if (line.Count != 0)
        {
            lines.Add([.. line]);
        }
        return new Layout(lines, lineWidth);
    }
}
