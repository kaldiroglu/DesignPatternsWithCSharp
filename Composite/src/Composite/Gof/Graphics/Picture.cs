namespace dev.kaldiroglu.Composite.Gof.Graphics;

/// <summary>
/// Composite role of the Composite pattern (GoF, p. 164).
/// </summary>
/// <remarks>
/// <para>
/// A <c>Picture</c> is a <see cref="Graphic"/> that is made of other
/// <c>Graphic</c>s. Its children may be primitives or other pictures, to any
/// depth — that recursion is the whole point of the pattern.
/// </para>
/// <para>
/// Notice how small <see cref="Draw"/> is: a composite implements the Component
/// operations by <em>forwarding them to its children</em>. There is no
/// conditional logic anywhere asking "is this a line or a picture?"; the type
/// system and the recursion do that work.
/// </para>
/// </remarks>
public class Picture(string name) : Graphic
{
    private readonly List<Graphic> _children = [];

    public string Name { get; } = name;

    public override void Draw(Point at)
    {
        Console.WriteLine(
            $"Picture \"{Name}\" drawing {_children.Count} child graphic(s) at {at}:");
        foreach (var child in _children)
        {
            child.Draw(at); // uniform: the child may be a leaf or another Picture
        }
    }

    public override void Add(Graphic child) => _children.Add(child);

    public override void Remove(Graphic child) => _children.Remove(child);

    public override Graphic GetChild(int index) => _children[index];

    public override IReadOnlyList<Graphic> Children => _children.AsReadOnly();

    public override bool IsComposite => true;
}
