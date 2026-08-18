namespace dev.kaldiroglu.Bridge.Gof.Problem;

/// <summary>A plain window on Presentation Manager. Draws with #, = and !.</summary>
public class PMWindow : Window
{
    public PMWindow(int width, int height) : base(width, height)
    {
    }

    public override void DrawRect(Canvas canvas, int x, int y, int w, int h) =>
        canvas.Rectangle(x, y, w, h, '#', '=', '!');

    public override void DrawText(Canvas canvas, int x, int y, string text) =>
        canvas.Text(x, y, text);

    public override string Platform => "PM";
}
