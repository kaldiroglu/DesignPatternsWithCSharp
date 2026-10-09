using System.Globalization;
using dev.kaldiroglu.ChainOfResponsibility.Expense.Problem;

namespace dev.kaldiroglu.ChainOfResponsibility.Expense.Solution;

/// <summary>
/// Runs the same six expenses through the three stages and through the chain.
/// <para>
/// The promise: every expense is approved by someone with enough authority, and never by
/// the person who spent it. Elif leads the team (1,000), Burak manages (10,000), Cem
/// directs (50,000), Deniz is the CFO (200,000). Emre is an engineer.
/// </para>
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/ChainOfResponsibility.Demo -- expense</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        List<string[]> claims =
        [
            ["Emre", "800", "books"],
            ["Emre", "5000", "a laptop"],
            ["Burak", "5000", "a conference"],
            ["Emre", "30000", "a server"],
            ["Cem", "30000", "a training course"],
            ["Emre", "500000", "a new office"]
        ];

        ApprovalService service = new ApprovalService();
        TeamLead lead = new TeamLead();
        LimitTable table = new LimitTable()
                .Add("Elif", 1_000).Add("Burak", 10_000).Add("Cem", 50_000).Add("Deniz", 200_000);

        AuditLog audit = new AuditLog();
        audit.Then(new Approver("Elif", 1_000))
                .Then(new Approver("Burak", 10_000))
                .Then(new Approver("Cem", 50_000))
                .Then(new Approver("Deniz", 200_000));

        foreach (string[] claim in claims)
        {
            string submitter = claim[0];
            int amount = int.Parse(claim[1], CultureInfo.InvariantCulture);
            Problem.Expense before = new Problem.Expense(submitter, amount, claim[2]);
            Expense after = new Expense(submitter, amount, claim[2]);
            Console.WriteLine(submitter + ", " + amount.ToString(CultureInfo.InvariantCulture) + ", " + claim[2]);
            Console.WriteLine("  stage one:   " + service.Approve(before));
            Console.WriteLine("  stage two:   " + lead.Approve(before));
            Console.WriteLine("  stage three: " + table.Approve(before));
            Console.WriteLine("  chain:       " + audit.Handle(after));
        }
        Console.WriteLine("The audit log saw "
                + audit.Seen.Count.ToString(CultureInfo.InvariantCulture) + " expenses.");
    }
}
