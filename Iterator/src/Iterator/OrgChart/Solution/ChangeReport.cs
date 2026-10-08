using dev.kaldiroglu.Iterator.OrgChart.Domain;

namespace dev.kaldiroglu.Iterator.OrgChart.Solution;

/// <summary>
/// "What changed after the reorganization?" — answered with two iterators.
/// </summary>
/// <remarks>
/// Compare <c>Problem.ChangeReport</c>, which had to copy both departments into lists. Here
/// the report holds one iterator for each chart and moves them forward together. It stops at
/// the first difference, so it never reads the rest of either chart.
/// </remarks>
public sealed class ChangeReport
{
    /// <summary>The first difference, or <c>null</c> when the charts are the same.</summary>
    public string? FirstDifference(Department before, Department after)
    {
        using IEnumerator<Employee> old = before.GetEnumerator();
        using IEnumerator<Employee> current = after.GetEnumerator();

        while (true)
        {
            bool oldHasNext = old.MoveNext();
            bool currentHasNext = current.MoveNext();
            if (!oldHasNext || !currentHasNext)
            {
                // One chart has ended. If the other has not, the sizes differ.
                return oldHasNext || currentHasNext ? "the charts have different sizes" : null;
            }

            Employee was = old.Current;
            Employee @is = current.Current;
            if (!was.Equals(@is))
            {
                return was + " -> " + @is;
            }
        }
    }
}
