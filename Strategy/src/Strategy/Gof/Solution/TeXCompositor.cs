namespace dev.kaldiroglu.Strategy.Gof.Solution;

/// <summary>
/// A <b>ConcreteStrategy</b>: GoF's <c>TeXCompositor</c>, which "implements the TeX
/// algorithm for finding linebreaks. This strategy tries to optimize linebreaks globally,
/// that is, one paragraph at a time" (p. 316).
/// <para>
/// The whole paragraph is read before anything is decided. It first asks how many lines a
/// greedy pass needs, then finds the narrowest column that still fits in that many lines,
/// and lays the text out to <em>that</em> width. The result uses no more lines than the
/// greedy algorithm and spreads the slack across all of them instead of dumping it on the
/// last.
/// </para>
/// <para>
/// That is the point of GoF's example: two algorithms with the same job, the same inputs and
/// nothing in common inside — and a composition that cannot tell which it is holding.
/// </para>
/// </summary>
public sealed class TeXCompositor : ICompositor
{
    public string Name => "TeXCompositor";

    public Layout Compose(IReadOnlyList<Component> components, int lineWidth)
    {
        var lines = Greedy(components, lineWidth).Count;
        var widest = components.Count == 0 ? 1 : components.Max(component => component.Width);

        // The narrowest column that still fits the paragraph into the same number of lines.
        var best = lineWidth;
        for (var width = widest; width <= lineWidth; width++)
        {
            if (Greedy(components, width).Count <= lines)
            {
                best = width;
                break;
            }
        }
        return new Layout(Greedy(components, best), lineWidth);
    }

    /// <summary>Fill each line until the next component will not fit — the pass TeX runs repeatedly.</summary>
    private static List<IReadOnlyList<Component>> Greedy(IReadOnlyList<Component> components, int lineWidth)
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
        return lines;
    }
}
