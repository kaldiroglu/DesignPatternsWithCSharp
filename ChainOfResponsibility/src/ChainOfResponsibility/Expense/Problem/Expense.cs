namespace dev.kaldiroglu.ChainOfResponsibility.Expense.Problem;

/// <summary>An expense claim: who spent the money, how much, and what for.</summary>
public sealed record Expense(string Submitter, int Amount, string Purpose);
