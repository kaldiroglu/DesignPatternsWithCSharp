namespace dev.kaldiroglu.ChainOfResponsibility.Expense.ListChain;

using Expense = dev.kaldiroglu.ChainOfResponsibility.Expense.Solution.Expense;

/// <summary>
/// Runs the six expenses through the chain kept as a list: the audit log first, then the
/// four approvers. The answers are the same as the chain with a next field.
/// </summary>
/// <remarks>
/// Run it with <c>dotnet run --project src/ChainOfResponsibility.Demo -- expense-list-chain</c>.
/// </remarks>
public static class ListChainMain
{
    public static void Run()
    {
        ApprovalChain chain = new ApprovalChain()
            .Then(new AuditLog())
            .Then(new Approver("Elif", 1_000))
            .Then(new Approver("Burak", 10_000))
            .Then(new Approver("Cem", 50_000))
            .Then(new Approver("Deniz", 200_000));

        List<Expense> expenses =
        [
            new Expense("Emre", 800, "books"),
            new Expense("Emre", 5_000, "a laptop"),
            new Expense("Burak", 5_000, "a conference"),
            new Expense("Emre", 30_000, "a server"),
            new Expense("Cem", 30_000, "a training course"),
            new Expense("Emre", 500_000, "a new office"),
        ];

        foreach (Expense expense in expenses)
        {
            Console.WriteLine(expense.Submitter + ", " + expense.Amount.ToString(System.Globalization.CultureInfo.InvariantCulture)
                + ", " + expense.Purpose + ": " + chain.Handle(expense));
        }
    }
}
