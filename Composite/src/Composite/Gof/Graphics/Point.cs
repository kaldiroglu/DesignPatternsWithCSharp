namespace dev.kaldiroglu.Composite.Gof.Graphics;

/// <summary>A position on the drawing canvas.</summary>
/// <param name="X">Horizontal coordinate.</param>
/// <param name="Y">Vertical coordinate.</param>
public readonly record struct Point(int X, int Y)
{
    /// <summary>Returns this point translated by the given offsets.</summary>
    public Point TranslatedBy(int dx, int dy) => new(X + dx, Y + dy);

    public override string ToString() => $"({X}, {Y})";
}
