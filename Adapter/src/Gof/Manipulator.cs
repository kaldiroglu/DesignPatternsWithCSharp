namespace dev.kaldiroglu.Adapter.Gof;

/// <summary>
/// GoF p. 146: a <c>Manipulator</c> knows how to animate a <see cref="Shape"/> in response to user
/// input (for example, dragging a handle). <see cref="Shape.CreateManipulator"/> is a factory method
/// - each pluggable returns the manipulator that understands it.
/// </summary>
public abstract class Manipulator
{
    public abstract void Manipulate();
}
