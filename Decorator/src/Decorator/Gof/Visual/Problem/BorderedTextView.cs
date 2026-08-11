namespace dev.kaldiroglu.Decorator.Gof.Visual.Problem;

/// <summary>A border, added by subclassing — so it lands on every instance of the class.</summary>
public class BorderedTextView : TextView
{
    public BorderedTextView(int contentWidth, int contentHeight, string text)
        : base(contentWidth, contentHeight, text)
    {
    }

    public override int Width() => ContentWidth() + 2;

    public override int Height() => ContentHeight() + 2;

    public override void Draw(Canvas canvas, int x, int y)
    {
        DrawText(canvas, x + 1, y + 1);
        canvas.Rectangle(x, y, Width(), Height());
    }
}
