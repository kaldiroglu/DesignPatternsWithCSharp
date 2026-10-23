using System.Globalization;
using dev.kaldiroglu.ChainOfResponsibility.Expense.Problem;

namespace dev.kaldiroglu.ChainOfResponsibility.Expense.Solution;

/// <summary>
/// Runs the same six expenses through the three stages and through the chain.
/// <para>
/// The promise: every expense is approved by someone with enough authority, and never by
/// the person who spent it.
/// </para>
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/ChainOfResponsibility.Demo -- expense-six-expenses</c>.
/// </remarks>
public static class SixExpensesMain
{
    public static void Run()
    {
        ApprovalService service = new ApprovalService();
        TeamLead lead = new TeamLead();
        LimitTable table = new LimitTable()
                .Add("Elif", 1_000).Add("Burak", 10_000).Add("Cem", 50_000).Add("Deniz", 200_000);
        AuditLog chain = SampleChain.Build();

        foreach (string[] claim in SampleChain.Claims())
        {
            string submitter = claim[0];
            int amount = int.Parse(claim[1], CultureInfo.InvariantCulture);
            Problem.Expense before = new Problem.Expense(submitter, amount, claim[2]);
            Expense after = new Expense(submitter, amount, claim[2]);
            Console.WriteLine(submitter + ", " + amount.ToString(CultureInfo.InvariantCulture) + ", " + claim[2]);
            Console.WriteLine("  stage one:   " + service.Approve(before));
            Console.WriteLine("  stage two:   " + lead.Approve(before));
            Console.WriteLine("  stage three: " + table.Approve(before));
            Console.WriteLine("  chain:       " + chain.Handle(after));
        }
    }
}
