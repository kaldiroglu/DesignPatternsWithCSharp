namespace dev.kaldiroglu.Bridge.Gof.Problem;

/// <summary>
/// An icon window on X.
/// <para>
/// Note what is in this class: nothing about icons. It inherits the icon behavior from
/// <see cref="IconWindow"/>, and then <i>copies the X drawing code out of
/// <see cref="XWindow"/></i>, because C# has no way to inherit from both. That copy is the
/// whole problem, and it happens once per (window kind x platform) pair.
/// </para>
/// </summary>
public class XIconWindow : IconWindow
{
    public XIconWindow(int width, int height, string label) : base(width, height, label)
    {
    }

    // Copied, character for character, from XWindow.
    public override void DrawRect(Canvas canvas, int x, int y, int w, int h) =>
        canvas.Rectangle(x, y, w, h, '+', '-', '|');

    public override void DrawText(Canvas canvas, int x, int y, string text) =>
        canvas.Text(x, y, text);

    public override string Platform => "X";
}
