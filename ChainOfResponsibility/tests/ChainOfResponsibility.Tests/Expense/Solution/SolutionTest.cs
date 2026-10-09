namespace dev.kaldiroglu.ChainOfResponsibility.Tests.Expense.Solution;

// These directives are inside the namespace on purpose. The test namespace
// Tests.Expense would otherwise hide the record type Expense.
using dev.kaldiroglu.ChainOfResponsibility.Expense.Solution;
using Xunit;
using static dev.kaldiroglu.ChainOfResponsibility.Tests.Printed;

/// <summary>The same six expenses through the chain. Every figure on the Part 3 slides is asserted here.</summary>
public class SolutionTest
{
    private static readonly IReadOnlyList<Expense> Six =
    [
        new Expense("Emre", 800, "books"),
        new Expense("Emre", 5_000, "a laptop"),
        new Expense("Burak", 5_000, "a conference"),
        new Expense("Emre", 30_000, "a server"),
        new Expense("Cem", 30_000, "a training course"),
        new Expense("Emre", 500_000, "a new office")
    ];

    /// <summary>The chain of Expense.Solution.Main: the audit log first, then the four approvers.</summary>
    private static AuditLog Chain()
    {
        AuditLog audit = new AuditLog();
        audit.Then(new Approver("Elif", 1_000))
            .Then(new Approver("Burak", 10_000))
            .Then(new Approver("Cem", 50_000))
            .Then(new Approver("Deniz", 200_000));
        return audit;
    }

    /// <summary>A link that records every expense above 20,000 and approves nothing.</summary>
    private sealed class FinanceCheck(List<Expense> checkedExpenses) : ExpenseHandler
    {
        protected override string? TryToHandle(Expense expense)
        {
            if (expense.Amount > 20_000)
            {
                checkedExpenses.Add(expense);
            }
            return null;
        }
    }

    /// <summary>The chain gives the six answers the rule asks for.</summary>
    [Fact]
    public void TheSixAnswers()
    {
        AuditLog chain = Chain();
        Assert.Equal(new[]
        {
            "approved by Elif",
            "approved by Burak",
            "approved by Cem",        // past Burak, who refuses his own
            "approved by Cem",
            "approved by Deniz",      // past Cem, who refuses his own
            "no one may approve it"
        }, Six.Select(chain.Handle).ToList());
    }

    /// <summary>The audit log saw 6 expenses, including the one nobody could approve.</summary>
    [Fact]
    public void TheAuditLogSawSix()
    {
        AuditLog chain = Chain();
        foreach (Expense expense in Six)
        {
            chain.Handle(expense);
        }
        Assert.Equal(6, chain.Seen.Count);
        Assert.Equal(Six, chain.Seen);
    }

    /// <summary>An expense of 500,000 passes every approver, and the end of the chain still answers.</summary>
    [Fact]
    public void TheEndOfTheChainAnswers()
    {
        Assert.Equal("no one may approve it", Chain().Handle(new Expense("Emre", 500_000, "a new office")));
        Assert.Equal("no one may approve it", new Approver("Elif", 1_000).Handle(new Expense("Emre", 2_000, "a desk")));
    }

    /// <summary>An approver checks both its limit and that the expense is not its own.</summary>
    [Fact]
    public void AnApproverHasBothRules()
    {
        Approver burak = new Approver("Burak", 10_000);
        Assert.Equal("approved by Burak", burak.Handle(new Expense("Emre", 10_000, "a laptop")));
        Assert.Equal("no one may approve it", burak.Handle(new Expense("Emre", 10_001, "a laptop")));
        Assert.Equal("no one may approve it", burak.Handle(new Expense("Burak", 5_000, "a conference")));
    }

    /// <summary>Today the chain gives Burak's 800 to Elif: within her limit, and not her own.</summary>
    [Fact]
    public void BuraksEightHundred()
    {
        Assert.Equal("approved by Elif", Chain().Handle(new Expense("Burak", 800, "a book")));
    }

    /// <summary>A finance check between Burak and Cem is one more link, and no approver changes.</summary>
    [Fact]
    public void AFinanceCheckIsOneMoreLink()
    {
        List<Expense> checkedExpenses = [];
        AuditLog audit = new AuditLog();
        audit.Then(new Approver("Elif", 1_000))
            .Then(new Approver("Burak", 10_000))
            .Then(new FinanceCheck(checkedExpenses))
            .Then(new Approver("Cem", 50_000))
            .Then(new Approver("Deniz", 200_000));

        Assert.Equal(new[]
        {
            "approved by Elif", "approved by Burak", "approved by Cem",
            "approved by Cem", "approved by Deniz", "no one may approve it"
        }, Six.Select(audit.Handle).ToList());
        // The 30,000 server, the 30,000 course and the 500,000 office.
        Assert.Equal(3, checkedExpenses.Count);
    }

    /// <summary>The submitter knows only the first link: no approver class names another.</summary>
    [Fact]
    public void NoLinkNamesAnother()
    {
        string approver = CodeOf("Expense/Solution/Approver.cs");
        string handler = CodeOf("Expense/Solution/ExpenseHandler.cs");
        foreach (string name in new[] { "Elif", "Burak", "Cem", "Deniz" })
        {
            Assert.Equal(0, CountOf(approver, name));
            Assert.Equal(0, CountOf(handler, name));
        }
        Assert.Equal(0, CountOf(approver, "new "));
    }

    /// <summary>Main prints each design's answer for the six expenses, then that the audit log saw 6.</summary>
    [Fact]
    public void MainOutput()
    {
        IReadOnlyList<string> lines = By(Main.Run);
        Assert.Equal(new[]
        {
            "Emre, 800, books",
            "  stage one:   approved by Elif",
            "  stage two:   approved by Elif",
            "  stage three: approved by Elif",
            "  chain:       approved by Elif",
            "Emre, 5000, a laptop",
            "  stage one:   approved by Burak",
            "  stage two:   approved by Burak",
            "  stage three: approved by Burak",
            "  chain:       approved by Burak",
            "Burak, 5000, a conference",
            "  stage one:   approved by Cem",
            "  stage two:   approved by Cem",
            "  stage three: approved by Burak",
            "  chain:       approved by Cem",
            "Emre, 30000, a server",
            "  stage one:   approved by Cem",
            "  stage two:   approved by Cem",
            "  stage three: approved by Cem",
            "  chain:       approved by Cem",
            "Cem, 30000, a training course",
            "  stage one:   approved by Deniz",
            "  stage two:   approved by Deniz",
            "  stage three: approved by Cem",
            "  chain:       approved by Deniz",
            "Emre, 500000, a new office",
            "  stage one:   no one may approve it",
            "  stage two:   no one may approve it",
            "  stage three: no one may approve it",
            "  chain:       no one may approve it",
            "The audit log saw 6 expenses."
        }, lines);
    }
}
