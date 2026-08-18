namespace dev.kaldiroglu.Bridge.Shape.Solution;

/// <summary>
/// The Abstraction: a shape that holds a device and never asks which one it has.
/// <para>
/// Every subclass expresses itself purely in <see cref="IShapeDrawer"/> primitives. That is the
/// discipline the solution asks for, and the payback is that <see cref="Triangle"/> was added
/// later without a single drawer being touched.
/// </para>
/// </summary>
public abstract class AbstractShape : IShape
{
    protected IShapeDrawer Drawer;

    protected AbstractShape(string name, IShapeDrawer drawer)
    {
        Name = name;
        Drawer = drawer ?? throw new ArgumentNullException(
            nameof(drawer), "a shape must have something to draw on");
    }

    public string Name { get; }

    public abstract void Draw();

    public abstract void Erase();

    public void SetDrawer(IShapeDrawer drawer) =>
        Drawer = drawer ?? throw new ArgumentNullException(nameof(drawer));
}
