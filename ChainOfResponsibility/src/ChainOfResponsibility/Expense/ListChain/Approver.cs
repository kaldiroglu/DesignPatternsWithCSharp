namespace dev.kaldiroglu.ChainOfResponsibility.Expense.ListChain;

using Expense = dev.kaldiroglu.ChainOfResponsibility.Expense.Solution.Expense;

/// <summary>Approves if the amount is within its limit and the expense is not its own.</summary>
public sealed record Approver(string Name, int Limit) : ILink
{
    public string? TryToHandle(Expense expense)
    {
        if (expense.Amount <= Limit && expense.Submitter != Name)
        {
            return "approved by " + Name;
        }
        return null;
    }
}
