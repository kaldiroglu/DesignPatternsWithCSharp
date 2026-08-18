namespace dev.kaldiroglu.Bridge.Gof.Problem;

/// <summary>
/// A third kind of window — a transient dialog with a title bar.
/// <para>
/// Adding it is where the arithmetic becomes visible: this one abstract class forces one new
/// leaf class <i>per platform</i>, and neither of them contains anything new.
/// </para>
/// </summary>
public abstract class TransientWindow : Window
{
    protected TransientWindow(int width, int height, string title) : base(width, height) =>
        Title = title;

    protected string Title { get; }

    public override void DrawContents(Canvas canvas)
    {
        DrawBorder(canvas);
        DrawText(canvas, 2, 0, $" {Title} ");
    }
}
