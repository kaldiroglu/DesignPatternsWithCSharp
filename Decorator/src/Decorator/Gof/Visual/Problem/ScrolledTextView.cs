namespace dev.kaldiroglu.Decorator.Gof.Visual.Problem;

/// <summary>A scrollbar, added the same way — and unreachable from the border subclass.</summary>
public class ScrolledTextView : TextView
{
    public ScrolledTextView(int contentWidth, int contentHeight, string text)
        : base(contentWidth, contentHeight, text)
    {
    }

    public override int Width() => ContentWidth() + 1;

    public override int Height() => ContentHeight();

    public override void Draw(Canvas canvas, int x, int y)
    {
        DrawText(canvas, x, y);
        DrawScrollbar(canvas, x + ContentWidth(), y, Height());
    }

    private static void DrawScrollbar(Canvas canvas, int x, int y, int height)
    {
        canvas.Put(x, y, '^');
        for (var i = 1; i < height - 1; i++)
        {
            canvas.Put(x, y + i, '#');
        }

        canvas.Put(x, y + height - 1, 'v');
    }
}
