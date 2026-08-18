namespace dev.kaldiroglu.Bridge.Shape.Solution;

/// <summary>
/// The Abstraction's interface: what a shape can do, whichever device it is drawn on.
/// <para>
/// <c>SetDrawer</c> is what makes this a Bridge rather than a Strategy chosen once: the device
/// can be changed on an object that already exists, mid-program.
/// </para>
/// </summary>
public interface IShape
{
    void Draw();

    void Erase();

    void SetDrawer(IShapeDrawer drawer);
}
