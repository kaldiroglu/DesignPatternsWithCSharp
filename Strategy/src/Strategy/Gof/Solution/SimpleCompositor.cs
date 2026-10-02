namespace dev.kaldiroglu.Strategy.Gof.Solution;

/// <summary>
/// A <b>ConcreteStrategy</b>: GoF's <c>SimpleCompositor</c>, "which implements a simple
/// line breaking strategy that determines linebreaks one at a time" (p. 316).
/// <para>
/// Fill the line until the next component will not fit, then break. Fast, and it leaves
/// whatever gap it leaves.
/// </para>
/// </summary>
public sealed class SimpleCompositor : ICompositor
{
    public string Name => "SimpleCompositor";

    public Layout Compose(IReadOnlyList<Component> components, int lineWidth)
    {
        var lines = new List<IReadOnlyList<Component>>();
        var line = new List<Component>();
        var used = 0;
        foreach (var component in components)
        {
            var needed = line.Count == 0 ? component.Width : component.Width + 1;
            if (used + needed > lineWidth && line.Count != 0 && line[^1].Breakable)
            {
                lines.Add([.. line]);
                line.Clear();
                used = 0;
                needed = component.Width;
            }
            line.Add(component);
            used += needed;
        }
        lines.Add([.. line]);
        return new Layout(lines, lineWidth);
    }
}
