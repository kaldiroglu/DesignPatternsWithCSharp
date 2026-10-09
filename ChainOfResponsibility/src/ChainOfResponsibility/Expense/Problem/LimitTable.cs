namespace dev.kaldiroglu.ChainOfResponsibility.Expense.Problem;

/// <summary>
/// Stage three: a table of limits, set up when the program starts.
/// <para>
/// No names in the code, no order fixed in a class: add a row and there is a new level.
/// The table finds the approver with the smallest limit that covers the amount. But the
/// table only knows amounts. When the manager spends 5,000, the table sends it to the
/// manager — who approves an expense of their own. The rule "not your own expense" belongs
/// to the approver, and the table has no place for it.
/// </para>
/// </summary>
public sealed class LimitTable
{
    // Kept sorted by limit, as Java's TreeMap is.
    private readonly SortedDictionary<int, string> approvers = new();

    public LimitTable Add(string approver, int limit)
    {
        approvers[limit] = approver;
        return this;
    }

    public string Approve(Expense expense)
    {
        // The first row whose limit is at least the amount. This is what Java's
        // TreeMap.ceilingEntry finds.
        foreach (var row in approvers)
        {
            if (row.Key >= expense.Amount)
            {
                return "approved by " + row.Value;
            }
        }
        return "no one may approve it";
    }
}
