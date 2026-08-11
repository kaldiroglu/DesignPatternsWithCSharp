namespace dev.kaldiroglu.Decorator.Gof.Visual.Solution;

/// <summary>
/// The <b>Decorator</b>: it maintains a reference to a Component object and defines an
/// interface that conforms to Component's interface (GoF p. 178).
/// <para>
/// Two properties make the whole pattern work, and both are visible in this small class:
/// it <i>is</i> an <see cref="IVisualComponent"/>, so a decorated object can be used
/// anywhere an undecorated one can — including inside another decorator; and it
/// <i>has</i> one, and forwards to it, so by default it changes nothing.
/// </para>
/// </summary>
public abstract class Decorator : IVisualComponent
{
    private readonly IVisualComponent _component;

    protected Decorator(IVisualComponent component) =>
        _component = component ?? throw new ArgumentNullException(
            nameof(component), "a decorator must decorate something");

    /// <summary>The wrapped component. Subclasses draw it, then add their own embellishment.</summary>
    protected IVisualComponent Component() => _component;

    public virtual int Width() => _component.Width();

    public virtual int Height() => _component.Height();

    public virtual void Draw(Canvas canvas, int x, int y) => _component.Draw(canvas, x, y);
}
