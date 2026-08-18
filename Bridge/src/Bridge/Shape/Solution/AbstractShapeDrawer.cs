namespace dev.kaldiroglu.Bridge.Shape.Solution;

/// <summary>
/// Shared state for the concrete drawers, and the measuring instrument for the tests.
/// <para>
/// Every device call is recorded, so a test can assert what a shape actually asked for rather
/// than describing it. Note that <see cref="Calls"/> is <i>not</i> on <see cref="IShapeDrawer"/>:
/// the implementor interface stays primitives-only, and this is a detail of how the examples
/// are observed.
/// </para>
/// </summary>
public abstract class AbstractShapeDrawer : IShapeDrawer
{
    private readonly List<string> _calls = [];

    protected AbstractShapeDrawer(string name) => Name = name;

    public string Name { get; }

    /// <summary>What this device was asked to do, in order.</summary>
    public IReadOnlyList<string> Calls => _calls.AsReadOnly();

    public void ResetCalls() => _calls.Clear();

    protected void Record(string call)
    {
        _calls.Add(call);
        Console.WriteLine($"  {Name}: {call}");
    }

    public abstract void DrawLine(int x1, int y1, int x2, int y2);

    public abstract void DrawArc(int centerX, int centerY, int radius, int startDegrees,
        int sweepDegrees);

    public abstract void Clear(int x, int y, int width, int height);
}
