namespace dev.kaldiroglu.Adapter.Gof;

/// <summary>The <see cref="Manipulator"/> a <see cref="LineShape"/> creates.</summary>
public class LineManipulator : Manipulator
{
    private readonly Shape owner;

    public LineManipulator(Shape owner)
    {
        this.owner = owner;
    }

    public override void Manipulate()
    {
        Console.WriteLine("    [LineManipulator] manipulating " + owner);
    }
}
