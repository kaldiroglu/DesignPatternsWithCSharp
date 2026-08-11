namespace dev.kaldiroglu.Decorator.Gof.Visual.Problem;

/// <summary>Both embellishments, in one order. The scrollbar had to be copied.</summary>
public class BorderedScrolledTextView : BorderedTextView
{
    public BorderedScrolledTextView(int contentWidth, int contentHeight, string text)
        : base(contentWidth, contentHeight, text)
    {
    }

    public override int Width() => ContentWidth() + 3; // 2 for the border, 1 for the scrollbar

    public override int Height() => ContentHeight() + 2;

    public override void Draw(Canvas canvas, int x, int y)
    {
        DrawText(canvas, x + 1, y + 1);
        DrawScrollbar(canvas, x + 1 + ContentWidth(), y + 1, ContentHeight());
        canvas.Rectangle(x, y, Width(), Height());
    }

    // Copied verbatim from ScrolledTextView. There is nowhere else to put it: this class
    // already inherits from BorderedTextView, and a protected member of a sibling class
    // is not accessible. This duplication is the cost of embellishing by subclassing.
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
