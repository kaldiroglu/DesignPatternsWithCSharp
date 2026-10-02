namespace dev.kaldiroglu.Strategy.Sorting.Subclassing;

/// <summary>
/// Stage two: each algorithm becomes a subclass.
/// <para>
/// A real improvement. Each algorithm is now readable and testable on its own, and a fourth
/// is a new file rather than a fourth branch in a method that already works for three.
/// </para>
/// <para>
/// What it decides quietly is that <b>a sorter is its algorithm</b>. Whoever holds one has
/// already chosen, so the choice moves out to every caller — and the caller is the one place
/// that knows nothing about which algorithm suits which input.
/// </para>
/// </summary>
public abstract class Sorter
{
    public abstract string Name { get; }

    public abstract void Sort(double[] list);
}
