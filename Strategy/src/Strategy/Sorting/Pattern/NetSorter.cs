namespace dev.kaldiroglu.Strategy.Sorting.Pattern;

/// <summary>
/// Hand it to the library, which is the right answer once the array is big enough.
/// <para>
/// here the library is <see cref="Array.Sort(Array)"/>.
/// </para>
/// </summary>
public sealed class NetSorter : ISorter
{
    public string Name => "JavaSort";

    public void Sort(double[] list)
    {
        Array.Sort(list);
    }
}
