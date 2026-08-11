namespace dev.kaldiroglu.Decorator.Gof.Visual.SkinAndGuts;

/// <summary>A <b>ConcreteStrategy</b>: every other cell is left blank.</summary>
public sealed class DashedBorder : IBorderStyle
{
    public void Stroke(Canvas canvas, int x, int y, int width, int height)
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
}
