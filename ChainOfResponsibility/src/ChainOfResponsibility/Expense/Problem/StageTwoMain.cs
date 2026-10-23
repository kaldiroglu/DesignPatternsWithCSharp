namespace dev.kaldiroglu.ChainOfResponsibility.Expense.Problem;

/// <summary>Stage two: each approver decides, and creates the next one by class.</summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/ChainOfResponsibility.Demo -- expense-problem-stage-two</c>.
/// </remarks>
public static class StageTwoMain
{
    public static void Run()
    {
        TeamLead lead = new TeamLead();
        foreach (Expense expense in SampleExpenses.All())
        {
            Console.WriteLine(SampleExpenses.Header(expense));
            Console.WriteLine("  stage two:   " + lead.Approve(expense));
        }
    }
}
