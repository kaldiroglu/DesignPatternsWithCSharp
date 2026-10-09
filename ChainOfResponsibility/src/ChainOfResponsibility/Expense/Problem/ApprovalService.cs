namespace dev.kaldiroglu.ChainOfResponsibility.Expense.Problem;

/// <summary>
/// Stage one: one method decides who approves.
/// <para>
/// It is correct: nobody approves above their limit, and nobody approves their own expense.
/// But every approver's name and limit is written here, and every rule is one more
/// condition in this method. A new level, or a person on leave, is an edit to this class.
/// </para>
/// </summary>
public sealed class ApprovalService
{
    public string Approve(Expense expense)
    {
        int amount = expense.Amount;
        string submitter = expense.Submitter;
        if (amount <= 1_000 && submitter != "Elif")
        {
            return "approved by Elif";
        }
        else if (amount <= 10_000 && submitter != "Burak")
        {
            return "approved by Burak";
        }
        else if (amount <= 50_000 && submitter != "Cem")
        {
            return "approved by Cem";
        }
        else if (amount <= 200_000 && submitter != "Deniz")
        {
            return "approved by Deniz";
        }
        else
        {
            return "no one may approve it";
        }
    }
}
