namespace dev.kaldiroglu.Bridge.Shape.Problem;

/// <summary>One shape kind. It cannot draw itself: only its per-device subclasses can.</summary>
public abstract class Rectangle : AbstractShape
{
    protected Rectangle(string name) : base(name)
    {
    }
}
