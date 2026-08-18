namespace dev.kaldiroglu.Bridge.Gof.Problem;

/// <summary>
/// A second reason to subclass arrives: a window that shows an icon and a label.
/// <para>
/// This is where the design breaks. "Icon window" is a kind of window; "X window" is also a
/// kind of window; and a class can only extend one of them. The two leaf classes below
/// therefore have to repeat their platform's drawing code, verbatim.
/// </para>
/// </summary>
public abstract class IconWindow : Window
{
    protected IconWindow(int width, int height, string label) : base(width, height) =>
        Label = label;

    protected string Label { get; }

    public override void DrawContents(Canvas canvas)
    {
        DrawBorder(canvas);
        DrawRect(canvas, 2, 1, 3, 2);          // the icon
        DrawText(canvas, 6, 2, Label);
    }
}
