namespace dev.kaldiroglu.Decorator.Gof.Visual.SkinAndGuts;

/// <summary>A <b>ConcreteStrategy</b>: one character, all the way round.</summary>
public sealed class ThickBorder : IBorderStyle
{
    public void Stroke(Canvas canvas, int x, int y, int width, int height)
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
