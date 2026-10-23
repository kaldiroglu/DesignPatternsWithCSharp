namespace dev.kaldiroglu.Strategy.Sorting.Subclassing;

/// <summary>
/// Hand it to the library, which is the right answer once the array is big enough.
/// <para>
/// It reports the name <c>JavaSort</c>, as the sorter in the other two sorting stages does;
/// the library is <see cref="Array.Sort(Array)"/>.
/// </para>
/// </summary>
public sealed class BuiltInSorter : Sorter
{
    public override string Name => "JavaSort";

    public override void Sort(double[] list)
    {
        Array.Sort(list);
    }
}
