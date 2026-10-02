namespace dev.kaldiroglu.Strategy.Sorting.Pattern;

/// <summary>
/// The <b>Strategy</b>: one way of sorting an array.
/// <para>
/// The same three algorithms as the two stages before it, and the same code inside them. What
/// changed is who holds them: nothing extends this, and nothing that holds one had to choose
/// it.
/// </para>
/// </summary>
public interface ISorter
{
    string Name { get; }

    void Sort(double[] list);
}
