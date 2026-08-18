namespace dev.kaldiroglu.Bridge.Gof.Solution;

/// <summary>
/// A second <b>RefinedAbstraction</b>. Adding it cost exactly one class — and it works on every
/// platform that exists now, and on every platform added later.
/// </summary>
public sealed class TransientWindow : Window
{
    private readonly string _title;

    public TransientWindow(int width, int height, string title, IWindowImp imp)
        : base(width, height, imp) =>
        _title = title;

    public override void DrawContents(Canvas canvas)
    {
        DrawBorder(canvas);
        DrawText(canvas, 2, 0, $" {_title} ");
    }
}
