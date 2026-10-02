namespace dev.kaldiroglu.Strategy.Sorting.Pattern;

/// <summary>
/// Hand it to the library, which is the right answer once the array is big enough.
/// <para>
/// The name is the Java original's, kept so that every stage reports the same three names;
/// here the library is <see cref="Array.Sort(Array)"/>.
/// </para>
/// </summary>
public sealed class JavaSorter : ISorter
{
    public string Name => "JavaSort";

    public void Sort(double[] list)
    {
        Array.Sort(list);
    }
}
