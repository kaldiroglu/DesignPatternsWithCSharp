namespace dev.kaldiroglu.ChainOfResponsibility.Expense.Solution;

/// <summary>
/// A <b>ConcreteHandler</b> that never decides: it records every expense and passes it on.
/// <para>
/// It is put at the front of the chain without changing any approver. A link may do some
/// work and still pass the request on.
/// </para>
/// </summary>
public sealed class AuditLog : ExpenseHandler
{
    private readonly List<Expense> seen = [];

    protected override string? TryToHandle(Expense expense)
    {
        seen.Add(expense);
        return null;
    }

    /// <summary>A copy of the expenses seen so far, as Java's <c>List.copyOf</c> gives.</summary>
    public IReadOnlyList<Expense> Seen => seen.ToList();
}
