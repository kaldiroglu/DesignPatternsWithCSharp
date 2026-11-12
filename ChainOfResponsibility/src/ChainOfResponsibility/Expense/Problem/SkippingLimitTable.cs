namespace dev.kaldiroglu.ChainOfResponsibility.Expense.Problem;

/// <summary>
/// Stage three, repaired in the table: when the approver the table finds is the submitter,
/// the table moves on to the next higher limit.
/// <para>
/// The answers are now right. But one method still decides for every approver: an approver
/// here is only a name, so a rule for one approver puts that name in <see cref="Approve"/>.
/// The loop walks the rows in order of limit, as a chain walks its links, but no approver
/// decides anything.
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
