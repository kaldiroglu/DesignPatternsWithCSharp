namespace dev.kaldiroglu.Bridge.Shape.Problem;

/// <summary>
/// What a shape can do.
/// <para>
/// The interface is not the problem. Everything that goes wrong in this namespace goes wrong
/// in the hierarchy underneath it, where the <i>device</i> is a base class.
/// </para>
/// </summary>
public interface IShape
{
    void Draw();

    void Erase();
}
