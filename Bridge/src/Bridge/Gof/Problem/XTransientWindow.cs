namespace dev.kaldiroglu.Bridge.Gof.Problem;

/// <summary>A transient window on X. The X drawing code, for the third time.</summary>
public class XTransientWindow : TransientWindow
{
    public XTransientWindow(int width, int height, string title) : base(width, height, title)
    {
    }

    public override void DrawRect(Canvas canvas, int x, int y, int w, int h) =>
        canvas.Rectangle(x, y, w, h, '+', '-', '|');

    public override void DrawText(Canvas canvas, int x, int y, string text) =>
        canvas.Text(x, y, text);

    public override string Platform => "X";
}
