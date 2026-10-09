namespace dev.kaldiroglu.ChainOfResponsibility.Tests.Expense.Problem;

// These directives are inside the namespace on purpose. The test namespace
// Tests.Expense would otherwise hide the record type Expense.
using dev.kaldiroglu.ChainOfResponsibility.Expense.Problem;
using Xunit;
using static dev.kaldiroglu.ChainOfResponsibility.Tests.Printed;

/// <summary>
/// The three attempts of Part 1, on the six expenses the slides use. Elif leads the team
/// (1,000), Burak manages (10,000), Cem directs (50,000), Deniz is the CFO (200,000).
/// </summary>
public class ProblemTest
{
    private const string Source = "Expense/Problem/";

    /// <summary>The six expenses of Expense.Solution.Main, in its order.</summary>
    internal static readonly IReadOnlyList<Expense> Six =
    [
        new Expense("Emre", 800, "books"),
        new Expense("Emre", 5_000, "a laptop"),
        new Expense("Burak", 5_000, "a conference"),
        new Expense("Emre", 30_000, "a server"),
        new Expense("Cem", 30_000, "a training course"),
        new Expense("Emre", 500_000, "a new office")
    ];

    /// <summary>What the rule says for the six expenses: never your own, never above your limit.</summary>
    internal static readonly IReadOnlyList<string> TheRuleSays =
    [
        "approved by Elif",
        "approved by Burak",
        "approved by Cem",
        "approved by Cem",
        "approved by Deniz",
        "no one may approve it"
    ];

    private static readonly string[] Names = ["Elif", "Burak", "Cem", "Deniz"];

    private static LimitTable Table() =>
        new LimitTable().Add("Elif", 1_000).Add("Burak", 10_000).Add("Cem", 50_000).Add("Deniz", 200_000);

    /// <summary>Stage one keeps the rule for all six expenses.</summary>
    [Fact]
    public void StageOneIsCorrect()
    {
        ApprovalService service = new ApprovalService();
        Assert.Equal(TheRuleSays, Six.Select(service.Approve).ToList());
    }

    /// <summary>Stage one writes every approver's name in one method.</summary>
    [Fact]
    public void StageOneNamesEveryone()
    {
        string code = CodeOf(Source + "ApprovalService.cs");
        foreach (string name in Names)
        {
            // The name is in the condition and in the answer.
            Assert.Equal(2, CountOf(code, name));
        }
    }

    /// <summary>Stage two keeps the rule for all six expenses.</summary>
    [Fact]
    public void StageTwoIsCorrect()
    {
        TeamLead lead = new TeamLead();
        Assert.Equal(TheRuleSays, Six.Select(lead.Approve).ToList());
    }

    /// <summary>In stage two each approver creates the next one by class, so the order is fixed in code.</summary>
    [Fact]
    public void StageTwoFixesTheOrder()
    {
        Assert.Equal(1, CountOf(CodeOf(Source + "TeamLead.cs"), "new Manager()"));
        Assert.Equal(1, CountOf(CodeOf(Source + "Manager.cs"), "new Director()"));
        Assert.Equal(1, CountOf(CodeOf(Source + "Director.cs"), "new Cfo()"));
        Assert.Equal(0, CountOf(CodeOf(Source + "Cfo.cs"), "new "));
    }

    /// <summary>Stage three has no names in its code: the names are rows added at startup.</summary>
    [Fact]
    public void StageThreeHasNoNames()
    {
        string code = CodeOf(Source + "LimitTable.cs");
        foreach (string name in Names)
        {
            Assert.Equal(0, CountOf(code, name));
        }
    }

    /// <summary>Stage three chooses by amount alone: Burak approves his own 5,000 and Cem his own 30,000.</summary>
    [Fact]
    public void StageThreeBreaksTheRule()
    {
        LimitTable table = Table();
        Assert.Equal(new[]
        {
            "approved by Elif",
            "approved by Burak",
            "approved by Burak",      // Burak's own conference
            "approved by Cem",
            "approved by Cem",        // Cem's own training course
            "no one may approve it"
        }, Six.Select(table.Approve).ToList());
    }

    /// <summary>5,000 is within the manager's limit of 10,000, and the manager is Burak.</summary>
    [Fact]
    public void BuraksConference()
    {
        Expense conference = new Expense("Burak", 5_000, "a conference");
        Assert.Equal("approved by Burak", Table().Approve(conference));
        Assert.Equal("approved by Cem", new ApprovalService().Approve(conference));
        Assert.Equal("approved by Cem", new TeamLead().Approve(conference));
    }

    /// <summary>Exactly two of the six expenses are approved by the person who spent the money in stage three.</summary>
    [Fact]
    public void TwoPeopleApprovedTheirOwn()
    {
        LimitTable table = Table();
        int own = Six.Count(e => table.Approve(e) == "approved by " + e.Submitter);
        Assert.Equal(2, own);
    }
}
