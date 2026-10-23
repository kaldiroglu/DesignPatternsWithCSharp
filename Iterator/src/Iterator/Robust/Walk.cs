namespace dev.kaldiroglu.Iterator.Robust;

/// <summary>
/// Walks a collection, makes one change after the first element, and says what the
/// enumerator did: it failed, or it went on and saw these elements.
/// </summary>
public static class Walk
{
    public static string ChangedAfterFirst(IEnumerable<string> collection, Action change)
    {
        var seen = new List<string>();
        try
        {
            foreach (var element in collection)
            {
                seen.Add(element);
                if (seen.Count == 1)
                {
                    change();
                }
            }
            return "no exception, saw " + Show(seen);
        }
        catch (InvalidOperationException)
        {
            return "InvalidOperationException after " + Show(seen);
        }
    }

    /// <summary>Prints a list as <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
