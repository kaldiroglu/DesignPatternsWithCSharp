namespace dev.kaldiroglu.Strategy.Gof.Solution;

/// <summary>
/// The <b>Context</b>: GoF's <c>Composition</c>, which "maintains a reference to a
/// Compositor object" (p. 316).
/// <para>
/// Compare with <c>Problem.Composition</c>. That class was a document that also typeset;
/// this one is a document that <em>asks</em>. The flag is gone, both algorithms are gone,
/// and what is left is the thing the class was always about: a list of components and the
/// width they have to fit into.
/// </para>
/// <para>
/// GoF, p. 316: "A composition maintains a collection of Component instances... When a
/// composition needs to reformat, it forwards this responsibility to its Compositor object."
/// </para>
/// </summary>
public sealed class Composition
{
    private readonly List<Component> _components = [];
    private readonly int _lineWidth;
    private ICompositor _compositor;

    public Composition(int lineWidth, ICompositor compositor)
    {
        _lineWidth = lineWidth;
        _compositor = compositor
            ?? throw new ArgumentNullException(nameof(compositor), "a composition needs a compositor");
    }

    public Composition Insert(Component component)
    {
        _components.Add(component);
        return this;
    }

    /// <summary>Change the algorithm on a document that already exists, and reformat.</summary>
    public void SetCompositor(ICompositor compositor)
    {
        ArgumentNullException.ThrowIfNull(compositor);
        _compositor = compositor;
    }

    public string CompositorName => _compositor.Name;

    /// <summary>GoF's <c>Repair()</c>: hand the work to whichever algorithm is in place.</summary>
    public Layout Repair() => _compositor.Compose([.. _components], _lineWidth);
}
