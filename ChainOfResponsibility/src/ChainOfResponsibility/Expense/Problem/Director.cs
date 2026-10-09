namespace dev.kaldiroglu.ChainOfResponsibility.Expense.Problem;

/// <summary>Stage two: the director, who knows the CFO by class.</summary>
public sealed class Director
{
    public string Approve(Expense expense)
    {
        if (expense.Amount <= 50_000 && expense.Submitter != "Cem")
        {
            return "approved by Cem";
        }
        return new Cfo().Approve(expense);
    }
}
