namespace dev.kaldiroglu.ChainOfResponsibility.Expense.Problem;

/// <summary>
/// Stage three: a table of limits chooses by amount alone, so Burak approves his own 5,000
/// and Cem his own 30,000.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/ChainOfResponsibility.Demo -- expense-problem-stage-three</c>.
/// </remarks>
public static class StageThreeMain
{
    public static void Run()
    {
        LimitTable table = new LimitTable()
                .Add("Elif", 1_000).Add("Burak", 10_000).Add("Cem", 50_000).Add("Deniz", 200_000);
        foreach (Expense expense in SampleExpenses.All())
        {
            string answer = table.Approve(expense);
            bool own = answer == "approved by " + expense.Submitter;
            Console.WriteLine(SampleExpenses.Header(expense));
            Console.WriteLine("  stage three: " + answer + (own ? "  <- his own expense" : ""));
        }
    }
}
