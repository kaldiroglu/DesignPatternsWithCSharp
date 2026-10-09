using dev.kaldiroglu.Iterator.OrgChart.Domain;

namespace dev.kaldiroglu.Iterator.OrgChart.Problem;

/// <summary>
/// "What changed after the reorganization?" — answered with stage three.
/// </summary>
/// <remarks>
/// The report must walk the old chart and the new chart side by side and stop at the first
/// place they differ. A callback cannot do that: each department runs its own walk to the
/// end. So the report copies both departments into lists first, and then compares the lists.
/// This is the copy that stage three was built to avoid.
/// </remarks>
public sealed class ChangeReport
{
    /// <summary>The first difference, or <c>null</c> when the charts are the same.</summary>
    public string? FirstDifference(CallbackDepartment before, CallbackDepartment after)
    {
        var old = new List<Employee>();
        before.ForEachMember(old.Add);          // copy the whole old chart
        var current = new List<Employee>();
        after.ForEachMember(current.Add);       // copy the whole new chart

        int shared = Math.Min(old.Count, current.Count);
        for (int i = 0; i < shared; i++)
        {
            if (!old[i].Equals(current[i]))
            {
                return old[i] + " -> " + current[i];
            }
        }

        if (old.Count != current.Count)
        {
            return "the charts have different sizes";
        }

        return null;
    }

    /// <summary>Every difference. Again both charts are copied whole before anything is compared.</summary>
    public List<string> AllDifferences(CallbackDepartment before, CallbackDepartment after)
    {
        var old = new List<Employee>();
        before.ForEachMember(old.Add);
        var current = new List<Employee>();
        after.ForEachMember(current.Add);

        var changes = new List<string>();
        int shared = Math.Min(old.Count, current.Count);
        for (int i = 0; i < shared; i++)
        {
            if (!old[i].Equals(current[i]))
            {
                changes.Add(old[i] + " -> " + current[i]);
            }
        }

        if (old.Count != current.Count)
        {
            changes.Add("the charts have different sizes");
        }

        return changes;
    }
}
