namespace dev.kaldiroglu.Composite.Drawing;

/// <summary>Shared state for anything drawable: what it is called, and what color it is.</summary>
public abstract class GraphicObject(string name, string color) : IGraphic
{
    protected readonly string Name = name;
    protected readonly string Color = color;

    public string GetName() => Name;

    public string GetColor() => Color;

    public abstract void Draw();

    public abstract void Erase();

    public abstract void Paint();

    /// <summary>A leaf is one shape. <see cref="Canvas"/> overrides this to add up its children.</summary>
    public virtual int ShapeCount() => 1;

    public override string ToString() => $"{GetType().Name} \"{Name}\", {Color}";
}
