namespace dev.kaldiroglu.Decorator.Gof.Visual.SkinAndGuts;

/// <summary>
/// The <b>Strategy</b>: how a border is drawn, as an object.
/// <para>
/// This is the "guts" half of GoF implementation issue 4, <i>changing the skin of an
/// object versus changing its guts</i> (p. 180). The border itself is a skin — a decorator
/// wraps a component that knows nothing about it. But <i>which</i> border to draw is a
/// decision inside the decorator, and a decorator that answers it with a branch has to be
/// edited for every new answer.
/// </para>
/// </summary>
public interface IBorderStyle
{
    /// <summary>Strokes the outline of a rectangle whose top-left corner is (x, y).</summary>
    void Stroke(Canvas canvas, int x, int y, int width, int height);
}
