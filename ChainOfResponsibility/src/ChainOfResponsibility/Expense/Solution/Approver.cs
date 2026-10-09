namespace dev.kaldiroglu.ChainOfResponsibility.Expense.Solution;

/// <summary>
/// A <b>ConcreteHandler</b>: a person with a limit.
/// <para>
/// The approver decides by its own rule: the amount is within the limit, and the expense
/// is not its own. Both conditions are here, in the link, so the chain needs no table and
/// the client needs no names.
/// </para>
/// </summary>
public sealed class Approver : ExpenseHandler
{
    private readonly string name;
    private readonly int limit;

    public Approver(string name, int limit)
    {
        this.name = name;
        this.limit = limit;
    }

    protected override string? TryToHandle(Expense expense)
    {
        if (expense.Amount <= limit && expense.Submitter != name)
        {
            return "approved by " + name;
        }
        return null;
    }
}
