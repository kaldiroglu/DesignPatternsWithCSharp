namespace dev.kaldiroglu.Adapter.Gof;

/// <summary>The <see cref="Manipulator"/> a <see cref="TextShape"/> creates (GoF p. 147).</summary>
public class TextManipulator : Manipulator
{
    private readonly Shape owner;

    public TextManipulator(Shape owner)
    {
        this.owner = owner;
    }

    public override void Manipulate()
    {
        Console.WriteLine("    [TextManipulator] manipulating " + owner);
    }
}
