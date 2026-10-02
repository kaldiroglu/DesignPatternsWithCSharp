namespace dev.kaldiroglu.Strategy.Gof.Problem;

/// <summary>
/// GoF's example before the pattern: the text object breaks its own lines.
/// <para>
/// Design Patterns, p. 315: "There are many algorithms for breaking a stream of text into
/// lines. Hard-wiring all such algorithms into the classes that require them isn't
/// desirable for several reasons." This class is that hard-wiring, and the three reasons
/// GoF give are visible in it.
/// </para>
/// <para><b>The three, in GoF's words and in this file:</b></para>
/// <list type="bullet">
///   <item><description><b>"Clients get more complex if they include the line breaking code."</b>
///       The method below is a text object that also happens to be a typesetter. Read how much
///       of it is about laying out lines and how little is about being a document.</description></item>
///   <item><description><b>"Different algorithms will be appropriate at different times."</b>
///       The <c>quality</c> flag is that sentence made into a parameter, and it is a branch
///       that every future algorithm has to be threaded through.</description></item>
///   <item><description><b>"It's difficult to add new algorithms or vary existing ones when line
///       breaking is an integral part of a Composition."</b> A third algorithm is a third
///       branch in a method that already works for two.</description></item>
/// </list>
/// </summary>
public sealed class Composition
{
    private readonly IReadOnlyList<Component> _components;
    private readonly int _lineWidth;
    private readonly bool _quality;

    public Composition(IReadOnlyList<Component> components, int lineWidth, bool quality)
    {
        _components = [.. components];
        _lineWidth = lineWidth;
        _quality = quality;
    }

    /// <summary>
    /// Break the components into lines.
    /// <para>
    /// Two algorithms, one method, one boolean. The flag is the whole problem: a caller who
    /// wants a third algorithm has nothing to pass, and the class cannot be extended to
    /// accept one without editing this method.
    /// </para>
    /// </summary>
    public Layout Repair() => _quality ? BestFit() : FirstFit();

    /// <summary>Fill each line until the next component will not fit. Fast, and leaves ragged gaps.</summary>
    private Layout FirstFit()
    {
        var lines = new List<IReadOnlyList<Component>>();
        var line = new List<Component>();
        var used = 0;
        foreach (var component in _components)
        {
            var needed = line.Count == 0 ? component.Width : component.Width + 1;
            if (used + needed > _lineWidth && line.Count != 0 && line[^1].Breakable)
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
        return new Layout(lines, _lineWidth);
    }

    /// <summary>
    /// Look at the whole paragraph and even the lines out, which is TeX's idea.
    /// <para>
    /// Written out here in the same class as the fast one, which is exactly what GoF object
    /// to: two unrelated algorithms sharing a file because one object needs both. Note that
    /// the greedy pass below is the <em>same</em> code as <see cref="FirstFit"/>, copied,
    /// because this algorithm needs it as a subroutine and there is nowhere to put it.
    /// </para>
    /// </summary>
    private Layout BestFit()
    {
        var lines = Greedy(_lineWidth).Count;
        var widest = _components.Count == 0 ? 1 : _components.Max(component => component.Width);
        var best = _lineWidth;
        for (var width = widest; width <= _lineWidth; width++)
        {
            if (Greedy(width).Count <= lines)
            {
                best = width;
                break;
            }
        }
        return new Layout(Greedy(best), _lineWidth);
    }

    private List<IReadOnlyList<Component>> Greedy(int width)
    {
        var lines = new List<IReadOnlyList<Component>>();
        var line = new List<Component>();
        var used = 0;
        foreach (var component in _components)
        {
            var needed = line.Count == 0 ? component.Width : component.Width + 1;
            if (used + needed > width && line.Count != 0 && line[^1].Breakable)
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
