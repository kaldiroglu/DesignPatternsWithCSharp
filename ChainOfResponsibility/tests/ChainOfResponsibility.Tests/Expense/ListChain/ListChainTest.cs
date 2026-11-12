namespace dev.kaldiroglu.ChainOfResponsibility.Tests.Expense.ListChain;

using System.Reflection;
using dev.kaldiroglu.ChainOfResponsibility.Expense.ListChain;
using dev.kaldiroglu.ChainOfResponsibility.Expense.Solution;
using Xunit;
using static dev.kaldiroglu.ChainOfResponsibility.Tests.Printed;
using Expense = dev.kaldiroglu.ChainOfResponsibility.Expense.Solution.Expense;
using Approver = dev.kaldiroglu.ChainOfResponsibility.Expense.ListChain.Approver;
using AuditLog = dev.kaldiroglu.ChainOfResponsibility.Expense.ListChain.AuditLog;
using LinkedApprover = dev.kaldiroglu.ChainOfResponsibility.Expense.Solution.Approver;
using LinkedAuditLog = dev.kaldiroglu.ChainOfResponsibility.Expense.Solution.AuditLog;

/// <summary>The chain kept as a list: the same answers as the chain with a next field, and no handler knows another.</summary>
public class ListChainTest
{
    private static readonly IReadOnlyList<Expense> Six =
    [
        new Expense("Emre", 800, "books"),
        new Expense("Emre", 5_000, "a laptop"),
        new Expense("Burak", 5_000, "a conference"),
        new Expense("Emre", 30_000, "a server"),
        new Expense("Cem", 30_000, "a training course"),
        new Expense("Emre", 500_000, "a new office"),
    ];

    private static ApprovalChain Chain(AuditLog audit) =>
        new ApprovalChain()
            .Then(audit)
            .Then(new Approver("Elif", 1_000))
            .Then(new Approver("Burak", 10_000))
            .Then(new Approver("Cem", 50_000))
            .Then(new Approver("Deniz", 200_000));

    private static LinkedAuditLog LinkedChain()
    {
        LinkedAuditLog audit = new LinkedAuditLog();
        audit.Then(new LinkedApprover("Elif", 1_000))
            .Then(new LinkedApprover("Burak", 10_000))
            .Then(new LinkedApprover("Cem", 50_000))
            .Then(new LinkedApprover("Deniz", 200_000));
        return audit;
    }

    /// <summary>The list gives the same six answers as the chain with a next field.</summary>
    [Fact]
    public void SameAnswersAsTheLinkedChain()
    {
        ApprovalChain list = Chain(new AuditLog());
        LinkedAuditLog linked = LinkedChain();
        Assert.Equal(Six.Select(linked.Handle).ToList(), Six.Select(list.Handle).ToList());
        Assert.Equal("approved by Cem", list.Handle(Six[2]));
    }

    /// <summary>No handler has a field that holds another handler.</summary>
    [Fact]
    public void NoHandlerKnowsAnother()
    {
        foreach (Type handler in new[] { typeof(Approver), typeof(AuditLog) })
        {
            FieldInfo[] fields = handler.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Assert.DoesNotContain(fields, f => typeof(ILink).IsAssignableFrom(f.FieldType));
        }
    }

    /// <summary>The passing is one loop in ApprovalChain.</summary>
    [Fact]
    public void TheLoopDoesThePassing()
    {
        string code = CodeOf("Expense/ListChain/ApprovalChain.cs");
        Assert.Equal(1, CountOf(code, "foreach (ILink link in links)"));
        Assert.Equal(1, CountOf(code, "link.TryToHandle(expense)"));
    }

    /// <summary>The order of the list is the order of the chain: with Cem first, Cem approves Emre's 800.</summary>
    [Fact]
    public void TheListOrderIsTheChainOrder()
    {
        ApprovalChain cemFirst = new ApprovalChain()
            .Then(new Approver("Cem", 50_000))
            .Then(new Approver("Elif", 1_000));
        Assert.Equal("approved by Cem", cemFirst.Handle(Six[0]));
    }

    /// <summary>The audit log sees all six expenses.</summary>
    [Fact]
    public void TheAuditLogSawSix()
    {
        AuditLog audit = new AuditLog();
        ApprovalChain chain = Chain(audit);
        foreach (Expense expense in Six)
        {
            chain.Handle(expense);
        }
        Assert.Equal(Six, audit.Seen);
    }

    /// <summary>ListChainMain prints the six answers.</summary>
    [Fact]
    public void MainOutput()
    {
        Assert.Equal(new[]
        {
            "Emre, 800, books: approved by Elif",
            "Emre, 5000, a laptop: approved by Burak",
            "Burak, 5000, a conference: approved by Cem",
            "Emre, 30000, a server: approved by Cem",
            "Cem, 30000, a training course: approved by Deniz",
            "Emre, 500000, a new office: no one may approve it"
        }, By(ListChainMain.Run));
    }
}
