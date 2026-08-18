namespace dev.kaldiroglu.Bridge.Gof.Problem;

/// <summary>An icon window on Presentation Manager. The same copy, from the other platform.</summary>
public class PMIconWindow : IconWindow
{
    public PMIconWindow(int width, int height, string label) : base(width, height, label)
    {
    }

    // Copied, character for character, from PMWindow.
    public override void DrawRect(Canvas canvas, int x, int y, int w, int h) =>
        canvas.Rectangle(x, y, w, h, '#', '=', '!');

    public override void DrawText(Canvas canvas, int x, int y, string text) =>
        canvas.Text(x, y, text);

    public override string Platform => "PM";
}
