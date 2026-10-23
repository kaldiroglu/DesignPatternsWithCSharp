namespace dev.kaldiroglu.ChainOfResponsibility.Expense.Problem;

/// <summary>Stage one: one method decides who approves each of the six expenses.</summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/ChainOfResponsibility.Demo -- expense-problem-stage-one</c>.
/// </remarks>
public static class StageOneMain
{
    public static void Run()
    {
        ApprovalService service = new ApprovalService();
        foreach (Expense expense in SampleExpenses.All())
        {
            Console.WriteLine(SampleExpenses.Header(expense));
            Console.WriteLine("  stage one:   " + service.Approve(expense));
        }
    }
}
