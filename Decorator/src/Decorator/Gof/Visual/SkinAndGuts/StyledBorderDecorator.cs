using dev.kaldiroglu.Decorator.Gof.Visual.Solution;

namespace dev.kaldiroglu.Decorator.Gof.Visual.SkinAndGuts;

/// <summary>
/// The same border, with the styling decision moved into an <see cref="IBorderStyle"/>.
/// <para>
/// Both patterns are present, doing different jobs — which is the point of GoF
/// implementation issue 4 (p. 180). <b>Decorator, the skin:</b> this class wraps a
/// component that is unaware of it. <b>Strategy, the guts:</b> this class <i>was</i>
/// designed with a hook and holds the object that fills it, so a fourth border style is a
/// new class and nothing here changes.
/// </para>
/// <para>
/// Note what is not in <see cref="Draw"/>: a branch. This class names no concrete style,
/// which SkinAndGutsTests asserts by reflection.
/// </para>
/// </summary>
public sealed class StyledBorderDecorator : Solution.Decorator
{
    private readonly IBorderStyle _style;

    public StyledBorderDecorator(IVisualComponent component, IBorderStyle style)
        : base(component) =>
        _style = style ?? throw new ArgumentNullException(nameof(style), "a border needs a style");

    public override int Width() => Component().Width() + 2;

    public override int Height() => Component().Height() + 2;

    public override void Draw(Canvas canvas, int x, int y)
    {
        Component().Draw(canvas, x + 1, y + 1);        // the skin: forward to what we wrap
        _style.Stroke(canvas, x, y, Width(), Height()); // the guts: ask, do not decide
    }
}
