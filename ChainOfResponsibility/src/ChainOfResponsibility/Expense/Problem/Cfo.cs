namespace dev.kaldiroglu.ChainOfResponsibility.Expense.Problem;

/// <summary>Stage two: the CFO, the last in the line.</summary>
public sealed class Cfo
{
    public string Approve(Expense expense)
    {
        if (expense.Amount <= 200_000 && expense.Submitter != "Deniz")
        {
            return "approved by Deniz";
        }
        return "no one may approve it";
    }
}
