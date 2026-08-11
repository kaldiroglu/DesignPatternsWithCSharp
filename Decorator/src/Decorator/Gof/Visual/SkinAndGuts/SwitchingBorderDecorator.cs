using dev.kaldiroglu.Decorator.Gof.Visual.Solution;

namespace dev.kaldiroglu.Decorator.Gof.Visual.SkinAndGuts;

/// <summary>
/// A border decorator that decides how to draw with a branch. It works, and it is the
/// design most people write first.
/// <para>
/// As a <i>decorator</i> it is correct: the component it wraps still knows nothing about
/// borders. The problem is one level down. Every border style this class will ever support
/// has to be named in <see cref="Style"/> and handled in <see cref="Draw"/>, so a fourth
/// style is an edit to a class the other three already depend on.
/// </para>
/// </summary>
public sealed class SwitchingBorderDecorator : Solution.Decorator
{
    /// <summary>The closed vocabulary. A new style cannot be added without changing this file.</summary>
    public enum Style
    {
        Solid,
        Dashed,
        Thick
    }

    private readonly Style _style;

    public SwitchingBorderDecorator(IVisualComponent component, Style style) : base(component) =>
        _style = style;

    public override int Width() => Component().Width() + 2;

    public override int Height() => Component().Height() + 2;

    public override void Draw(Canvas canvas, int x, int y)
    {
        Component().Draw(canvas, x + 1, y + 1);

        var width = Width();
        var height = Height();

        if (_style == Style.Solid)
        {
            canvas.Rectangle(x, y, width, height);
        }
        else if (_style == Style.Dashed)
        {
            for (var i = 1; i < width - 1; i++)
            {
                var c = i % 2 == 1 ? '-' : ' ';
                canvas.Put(x + i, y, c);
                canvas.Put(x + i, y + height - 1, c);
            }

            for (var i = 1; i < height - 1; i++)
            {
                var c = i % 2 == 1 ? '|' : ' ';
                canvas.Put(x, y + i, c);
                canvas.Put(x + width - 1, y + i, c);
            }

            canvas.Put(x, y, '+');
            canvas.Put(x + width - 1, y, '+');
            canvas.Put(x, y + height - 1, '+');
            canvas.Put(x + width - 1, y + height - 1, '+');
        }
        else if (_style == Style.Thick)
        {
            for (var i = 0; i < width; i++)
            {
                canvas.Put(x + i, y, '#');
                canvas.Put(x + i, y + height - 1, '#');
            }

            for (var i = 0; i < height; i++)
            {
                canvas.Put(x, y + i, '#');
                canvas.Put(x + width - 1, y + i, '#');
            }
        }
    }
}
