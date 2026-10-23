namespace dev.kaldiroglu.ChainOfResponsibility.Expense.Solution;

/// <summary>
/// The six claims and the chain that the solution's runnable classes use. Elif leads the
/// team (1,000), Burak manages (10,000), Cem directs (50,000), Deniz is the CFO (200,000).
/// Emre is an engineer.
/// </summary>
internal static class SampleChain
{
    internal static List<string[]> Claims() =>
    [
        ["Emre", "800", "books"],
        ["Emre", "5000", "a laptop"],
        ["Burak", "5000", "a conference"],
        ["Emre", "30000", "a server"],
        ["Cem", "30000", "a training course"],
        ["Emre", "500000", "a new office"]
    ];

    /// <summary>The audit log first, then the four approvers.</summary>
    internal static AuditLog Build()
    {
        AuditLog audit = new AuditLog();
        audit.Then(new Approver("Elif", 1_000))
                .Then(new Approver("Burak", 10_000))
                .Then(new Approver("Cem", 50_000))
                .Then(new Approver("Deniz", 200_000));
        return audit;
    }
}
