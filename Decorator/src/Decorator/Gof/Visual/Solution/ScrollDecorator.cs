namespace dev.kaldiroglu.Decorator.Gof.Visual.Solution;

/// <summary>
/// A <b>ConcreteDecorator</b>: adds a vertical scrollbar to whatever it wraps (GoF p. 181).
/// <para>
/// There is exactly one scrollbar implementation here. The Problem namespace needed three
/// copies of it.
/// </para>
/// </summary>
public sealed class ScrollDecorator : Decorator
{
    public ScrollDecorator(IVisualComponent component) : base(component)
    {
    }

    public override int Width() => Component().Width() + 1;

    public override void Draw(Canvas canvas, int x, int y)
    {
        Component().Draw(canvas, x, y);
        DrawScrollbar(canvas, x + Component().Width(), y);
    }

    private void DrawScrollbar(Canvas canvas, int x, int y)
    {
        var height = Height();
        canvas.Put(x, y, '^');
        for (var i = 1; i < height - 1; i++)
        {
            canvas.Put(x, y + i, '#');
        }

        canvas.Put(x, y + height - 1, 'v');
    }
}
