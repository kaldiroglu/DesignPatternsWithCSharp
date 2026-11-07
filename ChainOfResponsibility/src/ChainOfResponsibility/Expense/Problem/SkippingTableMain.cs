namespace dev.kaldiroglu.ChainOfResponsibility.Expense.Problem;

/// <summary>
/// Stage three repaired in the table: Burak's 5,000 goes to Cem, and Cem's 30,000 goes to
/// Deniz, as the rule says.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/ChainOfResponsibility.Demo -- expense-problem-skipping-table</c>.
/// </remarks>
public static class SkippingTableMain
{
    public static void Run()
    {
        SkippingLimitTable table = new SkippingLimitTable()
                .Add("Elif", 1_000).Add("Burak", 10_000).Add("Cem", 50_000).Add("Deniz", 200_000);
        foreach (Expense expense in SampleExpenses.All())
        {
            Console.WriteLine(SampleExpenses.Header(expense));
            Console.WriteLine("  skipping table: " + table.Approve(expense));
        }
    }
}
