namespace dev.kaldiroglu.Decorator.Gof.Visual.Problem;

/// <summary>
/// BEFORE: the class every embellishment has to subclass. GoF p. 175.
/// </summary>
public class TextView
{
    private readonly int _contentWidth;
    private readonly int _contentHeight;
    private readonly string _text;

    public TextView(int contentWidth, int contentHeight, string text)
    {
        _contentWidth = contentWidth;
        _contentHeight = contentHeight;
        _text = text;
    }

    public virtual int Width() => _contentWidth;

    public virtual int Height() => _contentHeight;

    public virtual void Draw(Canvas canvas, int x, int y) => DrawText(canvas, x, y);

    protected void DrawText(Canvas canvas, int x, int y)
    {
        var lines = TextLayout.Wrap(_text, _contentWidth, _contentHeight);
        for (var i = 0; i < lines.Count; i++)
        {
            canvas.Text(x, y + i, lines[i]);
        }
    }

    protected int ContentWidth() => _contentWidth;

    protected int ContentHeight() => _contentHeight;

    public static string Render(TextView view)
    {
        var canvas = new Canvas(view.Width(), view.Height());
        view.Draw(canvas, 0, 0);
        return canvas.ToString();
    }
}
