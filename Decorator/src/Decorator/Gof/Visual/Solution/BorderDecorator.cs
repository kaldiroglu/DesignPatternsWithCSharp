namespace dev.kaldiroglu.Decorator.Gof.Visual.Solution;

/// <summary>
/// A <b>ConcreteDecorator</b>: adds a border to whatever it wraps (GoF p. 181).
/// <para>
/// It knows nothing about text views. It will decorate a <see cref="TextView"/>, a
/// <see cref="ScrollDecorator"/>, or another BorderDecorator — because all it requires is
/// the <see cref="IVisualComponent"/> interface. That is the difference between "border
/// of a text view" and "border of anything".
/// </para>
/// </summary>
public sealed class BorderDecorator : Decorator
{
    public BorderDecorator(IVisualComponent component) : base(component)
    {
    }

    public override int Width() => Component().Width() + 2;

    public override int Height() => Component().Height() + 2;

    public override void Draw(Canvas canvas, int x, int y)
    {
        Component().Draw(canvas, x + 1, y + 1); // forward the request...
        DrawBorder(canvas, x, y);               // ...then add our own responsibility
    }

    private void DrawBorder(Canvas canvas, int x, int y) =>
        canvas.Rectangle(x, y, Width(), Height());
}
