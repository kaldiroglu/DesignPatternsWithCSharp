namespace dev.kaldiroglu.ChainOfResponsibility.Expense.Problem;

/// <summary>
/// Stage three, repaired in the table: when the approver the table finds is the submitter,
/// the table moves on to the next higher limit.
/// <para>
/// The answers are now right. But the rule "not your own expense" belongs to the approvers,
/// and here the table applies it, so every new rule about one approver is another condition
/// in <see cref="Approve"/>. And the loop asks one approver after another until one may take
/// the expense: a chain, kept inside the table.
/// </para>
/// </summary>
public sealed class SkippingLimitTable
{
    // Kept sorted by limit.
    private readonly SortedDictionary<int, string> approvers = new();

    public SkippingLimitTable Add(string approver, int limit)
    {
        approvers[limit] = approver;
        return this;
    }

    public string Approve(Expense expense)
    {
        // The first row whose limit is at least the amount and whose approver is not the submitter.
        foreach (var row in approvers)
        {
            if (row.Key >= expense.Amount && row.Value != expense.Submitter)
            {
                return "approved by " + row.Value;
            }
        }
        return "no one may approve it";
    }
}
