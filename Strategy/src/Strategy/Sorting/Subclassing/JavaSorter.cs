namespace dev.kaldiroglu.Strategy.Sorting.Subclassing;

/// <summary>
/// Hand it to the library, which is the right answer once the array is big enough.
/// <para>
/// The name is kept so that every stage reports the same three names; the library is
/// <see cref="Array.Sort(Array)"/>.
/// </para>
/// </summary>
public sealed class JavaSorter : Sorter
{
    public override string Name => "JavaSort";

    public override void Sort(double[] list)
    {
        Array.Sort(list);
    }
}
