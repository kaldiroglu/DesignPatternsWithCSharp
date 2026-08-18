namespace dev.kaldiroglu.Bridge.Shape.Problem;

/// <summary>
/// One cell of the grid: a triangle, on MacOS.
/// <para>
/// Adding a shape kind costs three classes — the abstract kind and one leaf per device — and
/// the next device costs one more leaf for every shape already on the menu. That is m x n,
/// growing.
/// </para>
/// </summary>
public class TriangleMacOS : Triangle
{
    public TriangleMacOS(string name) : base(name)
    {
    }

    public override void Draw() => Console.WriteLine("  MacOS: drawing a triangle.");

    public override void Erase() => Console.WriteLine("  MacOS: erasing a triangle.");
}
