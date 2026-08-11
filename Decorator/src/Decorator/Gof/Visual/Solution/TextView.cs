namespace dev.kaldiroglu.Decorator.Gof.Visual.Solution;

/// <summary>
/// The <b>ConcreteComponent</b>: the object to which responsibilities can be attached
/// (GoF p. 178).
/// <para>
/// Compare it with Problem.TextView: this class knows nothing about borders or
/// scrollbars, and — this is the point — it will never need to be changed when a new
/// embellishment is invented.
/// </para>
/// </summary>
public sealed class TextView : IVisualComponent
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

    public int Width() => _contentWidth;

    public int Height() => _contentHeight;

    public void Draw(Canvas canvas, int x, int y)
    {
        var lines = TextLayout.Wrap(_text, _contentWidth, _contentHeight);
        for (var i = 0; i < lines.Count; i++)
        {
            canvas.Text(x, y + i, lines[i]);
        }
    }
}
