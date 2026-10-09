namespace dev.kaldiroglu.ChainOfResponsibility.Expense.Solution;

/// <summary>
/// The <b>Handler</b>: one link in the chain. It knows only the next link, by this type.
/// <para>
/// <see cref="Handle"/> asks this link first. If it does not handle the expense, the expense
/// goes to the next link. If there is no next link, nobody handled it — GoF consequence 3
/// (receipt isn't guaranteed) — and the chain says so instead of failing.
/// </para>
/// </summary>
public abstract class ExpenseHandler
{
    private ExpenseHandler? next;

    /// <summary>Links the next handler and returns it, so a chain can be written in one line.</summary>
    public ExpenseHandler Then(ExpenseHandler next)
    {
        this.next = next;
        return next;
    }

    public string Handle(Expense expense)
    {
        string? answer = TryToHandle(expense);
        if (answer != null)
        {
            return answer;
        }
        if (next == null)
        {
            return "no one may approve it";
        }
        return next.Handle(expense);
    }

    /// <summary>Returns the answer, or <c>null</c> to pass the expense on.</summary>
    protected abstract string? TryToHandle(Expense expense);
}
