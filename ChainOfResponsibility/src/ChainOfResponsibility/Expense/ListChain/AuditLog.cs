namespace dev.kaldiroglu.ChainOfResponsibility.Expense.ListChain;

using Expense = dev.kaldiroglu.ChainOfResponsibility.Expense.Solution.Expense;

/// <summary>Records every expense and never answers, so the next handler always gets it.</summary>
public sealed class AuditLog : ILink
{
    private readonly List<Expense> seen = [];

    public string? TryToHandle(Expense expense)
    {
        seen.Add(expense);
        return null;
    }

    /// <summary>A copy of the expenses seen so far.</summary>
    public IReadOnlyList<Expense> Seen => seen.ToList();
}
