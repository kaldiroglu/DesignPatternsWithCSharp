namespace dev.kaldiroglu.Decorator.Gof.Visual.Problem;

/// <summary>
/// The same two embellishments in the other order — and therefore a fifth class, with the
/// scrollbar copied again. Two embellishments, five classes.
/// </summary>
public class ScrolledBorderedTextView : ScrolledTextView
{
    public ScrolledBorderedTextView(int contentWidth, int contentHeight, string text)
        : base(contentWidth, contentHeight, text)
    {
    }

    public override int Width() => ContentWidth() + 3;

    public override int Height() => ContentHeight() + 2;

    public override void Draw(Canvas canvas, int x, int y)
    {
        DrawText(canvas, x + 1, y + 1);
        canvas.Rectangle(x, y, ContentWidth() + 2, Height()); // copied from BorderedTextView
        DrawScrollbar(canvas, x + ContentWidth() + 2, y, Height());
    }

    // Copied from ScrolledTextView for the same reason as in BorderedScrolledTextView.
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
