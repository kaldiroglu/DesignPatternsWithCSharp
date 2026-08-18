namespace dev.kaldiroglu.Bridge.Shape.Problem;

/// <summary>
/// Everything a shape knows that has nothing to do with a device: its name.
/// </summary>
public abstract class AbstractShape : IShape
{
    protected AbstractShape(string name) => Name = name;

    public string Name { get; }

    public abstract void Draw();

    public abstract void Erase();

    public override string ToString() => $"{Name} ({GetType().Name})";
}
