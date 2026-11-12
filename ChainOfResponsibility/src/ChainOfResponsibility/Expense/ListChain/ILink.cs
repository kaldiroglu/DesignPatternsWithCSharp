namespace dev.kaldiroglu.ChainOfResponsibility.Expense.ListChain;

using Expense = dev.kaldiroglu.ChainOfResponsibility.Expense.Solution.Expense;

/// <summary>
/// One handler in a chain kept as a list. It decides by its own rule and returns the answer,
/// or <c>null</c> to let the next handler try. It has no field for the next handler: the
/// list holds the order, and <see cref="ApprovalChain"/> does the passing.
/// </summary>
public interface ILink
{
    string? TryToHandle(Expense expense);
}
