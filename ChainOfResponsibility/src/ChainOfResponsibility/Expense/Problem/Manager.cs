namespace dev.kaldiroglu.ChainOfResponsibility.Expense.Problem;

/// <summary>Stage two: the manager, who knows the director by class.</summary>
public sealed class Manager
{
    public string Approve(Expense expense)
    {
        if (expense.Amount <= 10_000 && expense.Submitter != "Burak")
        {
            return "approved by Burak";
        }
        return new Director().Approve(expense);
    }
}
