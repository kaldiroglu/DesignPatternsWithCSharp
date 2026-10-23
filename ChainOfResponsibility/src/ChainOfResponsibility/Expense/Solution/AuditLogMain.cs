using System.Globalization;

namespace dev.kaldiroglu.ChainOfResponsibility.Expense.Solution;

/// <summary>
/// Sends the six expenses into the chain, with the audit log at the front. The audit log sees
/// every one, including the one nobody may approve.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/ChainOfResponsibility.Demo -- expense-audit-log</c>.
/// </remarks>
public static class AuditLogMain
{
    public static void Run()
    {
        AuditLog audit = SampleChain.Build();
        foreach (string[] claim in SampleChain.Claims())
        {
            audit.Handle(new Expense(claim[0], int.Parse(claim[1], CultureInfo.InvariantCulture), claim[2]));
        }
        Console.WriteLine("The audit log saw "
                + audit.Seen.Count.ToString(CultureInfo.InvariantCulture) + " expenses.");
    }
}
