namespace dev.kaldiroglu.Composite.Drawing;

/// <summary>
/// The Composite: a canvas is drawable, and it holds drawables.
/// </summary>
/// <remarks>
/// Being <em>both</em> is what makes the tree work — a canvas can be added to another canvas,
/// because the thing being added only has to be an <see cref="IGraphic"/>. Every operation
/// here does its own bit and then forwards to the children, which is the pattern in four
/// methods.
/// </remarks>
public class Canvas(string name, string color) : GraphicObject(name, color), ICompositeGraphic
{
    private readonly List<IGraphic> _elements = [];

    public IReadOnlyCollection<IGraphic> Graphics => _elements.AsReadOnly();

    public void AddGraphic(IGraphic graphic) => _elements.Add(graphic);

    public void RemoveGraphic(IGraphic graphic) => _elements.Remove(graphic);

    public override void Draw()
    {
        Console.WriteLine($"Drawing canvas: {Name}");
        foreach (var element in _elements)
        {
            element.Draw();
        }
    }

    public override void Erase()
    {
        Console.WriteLine($"Erasing canvas: {Name}");
        foreach (var element in _elements)
        {
            element.Erase();
        }
    }

    public override void Paint()
    {
        Console.WriteLine($"Painting canvas: {Name}, color {Color}");
        foreach (var element in _elements)
        {
            element.Paint();
        }
    }

    /// <summary>
    /// The payoff: one number for a whole tree, and no caller writes a loop.
    /// </summary>
    /// <remarks>
    /// A leaf answers 1, a canvas adds up its children, and neither the client nor this class
    /// has to know how deep the tree goes.
    /// </remarks>
    public override int ShapeCount() => _elements.Sum(e => e.ShapeCount());

    public void ListGraphic() => ListGraphic("");

    /// <summary>
    /// Recurses, with indentation: a nested canvas prints its own contents rather than one
    /// line of its own. Listing a tree is the same shape as measuring one — do this node's
    /// part, then ask the children to do theirs.
    /// </summary>
    private void ListGraphic(string indent)
    {
        Console.WriteLine(indent + this);
        foreach (var element in _elements)
        {
            if (element is Canvas nested)
            {
                nested.ListGraphic(indent + "    ");
            }
            else
            {
                Console.WriteLine(indent + "    " + element);
            }
        }
    }
}
