namespace dev.kaldiroglu.Decorator.Gof.Visual.SkinAndGuts;

/// <summary>A <b>ConcreteStrategy</b>: an unbroken outline, <c>+ - |</c>.</summary>
public sealed class SolidBorder : IBorderStyle
{
    public void Stroke(Canvas canvas, int x, int y, int width, int height) =>
        canvas.Rectangle(x, y, width, height);
}
