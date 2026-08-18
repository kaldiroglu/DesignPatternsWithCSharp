namespace dev.kaldiroglu.Bridge.Shape.Problem;

/// <summary>
/// One cell of the grid: a circle, on XWindows.
/// <para>
/// Adding a shape kind costs three classes — the abstract kind and one leaf per device — and
/// the next device costs one more leaf for every shape already on the menu. That is m x n,
/// growing.
/// </para>
/// </summary>
public class CircleXWindows : Circle
{
    public CircleXWindows(string name) : base(name)
    {
    }

    public override void Draw() => Console.WriteLine("  XWindows: drawing a circle.");

    public override void Erase() => Console.WriteLine("  XWindows: erasing a circle.");
}
