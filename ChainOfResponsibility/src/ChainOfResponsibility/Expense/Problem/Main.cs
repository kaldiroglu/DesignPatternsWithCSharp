using System.Globalization;

namespace dev.kaldiroglu.ChainOfResponsibility.Expense.Problem;

/// <summary>
/// Runs the six expenses through the three stages. Stages one and two keep the rule; stage
/// three lets Burak approve his own 5,000 and Cem his own 30,000.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/ChainOfResponsibility.Demo -- expense-problem</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        List<Expense> expenses =
        [
            new Expense("Emre", 800, "books"),
            new Expense("Emre", 5_000, "a laptop"),
            new Expense("Burak", 5_000, "a conference"),
            new Expense("Emre", 30_000, "a server"),
            new Expense("Cem", 30_000, "a training course"),
            new Expense("Emre", 500_000, "a new office")
        ];

        ApprovalService service = new ApprovalService();
        TeamLead lead = new TeamLead();
        LimitTable table = new LimitTable()
                .Add("Elif", 1_000).Add("Burak", 10_000).Add("Cem", 50_000).Add("Deniz", 200_000);

        foreach (Expense expense in expenses)
        {
            string third = table.Approve(expense);
            bool own = third == "approved by " + expense.Submitter;
            Console.WriteLine(expense.Submitter + ", "
                    + expense.Amount.ToString(CultureInfo.InvariantCulture) + ", " + expense.Purpose);
            Console.WriteLine("  stage one:   " + service.Approve(expense));
            Console.WriteLine("  stage two:   " + lead.Approve(expense));
            Console.WriteLine("  stage three: " + third + (own ? "  <- his own expense" : ""));
        }
    }
}
