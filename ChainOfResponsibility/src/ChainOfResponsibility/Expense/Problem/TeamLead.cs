namespace dev.kaldiroglu.ChainOfResponsibility.Expense.Problem;

/// <summary>
/// Stage two: each approver decides, and calls the next one by class.
/// <para>
/// The rules are now with the people they belong to. But <c>TeamLead</c> creates a
/// <c>Manager</c>, the manager creates a <c>Director</c>, and so on: the order is fixed in
/// the code. A finance check between the manager and the director is an edit to
/// <c>Manager</c>.
/// </para>
/// </summary>
public sealed class TeamLead
{
    public string Approve(Expense expense)
    {
        if (expense.Amount <= 1_000 && expense.Submitter != "Elif")
        {
            return "approved by Elif";
        }
        return new Manager().Approve(expense);
    }
}
