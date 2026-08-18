namespace dev.kaldiroglu.Bridge.Gof.Solution;

/// <summary>
/// A <b>RefinedAbstraction</b> (GoF p. 154): extends what a window can do, and says nothing
/// whatever about any platform.
/// <para>
/// Compare with <c>Problem.XIconWindow</c> and <c>Problem.PMIconWindow</c>: this one class
/// replaces both of them, and would replace the third and fourth as well.
/// </para>
/// </summary>
public sealed class IconWindow : Window
{
    private readonly string _label;

    public IconWindow(int width, int height, string label, IWindowImp imp)
        : base(width, height, imp) =>
        _label = label;

    public override void DrawContents(Canvas canvas)
    {
        DrawBorder(canvas);
        DrawRect(canvas, 2, 1, 3, 2);
        DrawText(canvas, 6, 2, _label);
    }
}
