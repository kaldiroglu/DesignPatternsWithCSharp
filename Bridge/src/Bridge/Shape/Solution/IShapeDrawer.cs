namespace dev.kaldiroglu.Bridge.Shape.Solution;

/// <summary>
/// The Implementor: what a shape is allowed to ask of a drawing device.
/// <para>
/// Every method here is a primitive of the device, not an operation of a shape. That
/// distinction is the whole solution, and it is the one most often got wrong. An earlier
/// version of this interface had <c>DrawCircle</c> and <c>DrawRectangle</c> on it, which looks
/// harmless and is not: adding a <see cref="Triangle"/> would then have forced every drawer to
/// grow a <c>DrawTriangle</c>, and the two hierarchies would no longer be independent — which
/// is the only thing Bridge exists to buy.
/// </para>
/// <para>
/// GoF put it in one sentence on p. 154: the Implementor interface "doesn't have to correspond
/// exactly to Abstraction's interface; in fact the two interfaces can be quite different.
/// Typically the Implementor interface provides only primitive operations, and Abstraction
/// defines higher-level operations based on these primitives."
/// </para>
/// </summary>
public interface IShapeDrawer
{
    void DrawLine(int x1, int y1, int x2, int y2);

    void DrawArc(int centerX, int centerY, int radius, int startDegrees, int sweepDegrees);

    void Clear(int x, int y, int width, int height);
}
