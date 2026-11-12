namespace dev.kaldiroglu.ChainOfResponsibility.Tests.CallCenter;

using dev.kaldiroglu.ChainOfResponsibility.CallCenter;
using Xunit;
using static dev.kaldiroglu.ChainOfResponsibility.Tests.Printed;

/// <summary>
/// The call center: standard, then gold, then VIP. A higher desk also serves lower customers,
/// so more than one desk may answer a call, and the order of the desks decides which one does.
/// </summary>
public class CallCenterTest
{
    private static List<string> DesksAndAnswers(ICallTaker first, ICustomer customer) =>
        By(() => first.Answer(customer))
            .Where(line => line.EndsWith("received a customer.", StringComparison.Ordinal)
                           || line.StartsWith("Answer:", StringComparison.Ordinal))
            .ToList();

    /// <summary>A standard customer is answered by the standard desk.</summary>
    [Fact]
    public void Standard()
    {
        Assert.Equal(new[]
        {
            "StandardCallTaker received a customer.",
            "Answer: Here is your answer!"
        }, DesksAndAnswers(Main.CreateCallTakers(), new StandardCustomer()));
    }

    /// <summary>A gold customer passes the standard desk and is answered by the gold desk.</summary>
    [Fact]
    public void Gold()
    {
        Assert.Equal(new[]
        {
            "StandardCallTaker received a customer.",
            "GoldCallTaker received a customer.",
            "Answer: Here is your GOLD answer!"
        }, DesksAndAnswers(Main.CreateCallTakers(), new GoldCustomer()));
    }

    /// <summary>A VIP customer passes two desks and gets the VIP answer.</summary>
    [Fact]
    public void Vip()
    {
        Assert.Equal(new[]
        {
            "StandardCallTaker received a customer.",
            "GoldCallTaker received a customer.",
            "VipCallTaker received a customer.",
            "Answer: Here is your VIP answer!"
        }, DesksAndAnswers(Main.CreateCallTakers(), new VipCustomer()));
    }

    /// <summary>A higher desk serves lower customers: without the standard desk, gold answers a standard customer.</summary>
    [Fact]
    public void HigherDesksServeLowerCustomers()
    {
        ICallTaker first = new GoldCallTaker(new VipCallTaker(null));
        Assert.Equal(new[]
        {
            "GoldCallTaker received a customer.",
            "Answer: Here is your GOLD answer!"
        }, DesksAndAnswers(first, new StandardCustomer()));
        ICallTaker vipOnly = new VipCallTaker(null);
        foreach (ICustomer customer in Main.Customers())
        {
            Assert.Contains("Answer: Here is your VIP answer!", DesksAndAnswers(vipOnly, customer));
        }
    }

    /// <summary>The order matters: with the VIP desk first, it answers all three customers.</summary>
    [Fact]
    public void TheOrderMatters()
    {
        IReadOnlyList<string> lines = By(VipFirstMain.Run);
        Assert.Equal(3, lines.Count(line => line == "Answer: Here is your VIP answer!"));
        Assert.DoesNotContain("GoldCallTaker received a customer.", lines);
        Assert.DoesNotContain("StandardCallTaker received a customer.", lines);
    }

    /// <summary>A customer no desk serves reaches the end of the chain and is told so, with no exception.</summary>
    [Fact]
    public void NoDeskCanAnswer()
    {
        ICallTaker first = new StandardCallTaker(new GoldCallTaker(null));
        IReadOnlyList<string> lines = By(() => first.Answer(new VipCustomer()));
        Assert.Contains("Answer: No desk can answer you.", lines);
    }

    /// <summary>The VIP desk is the end of the chain and has no next desk.</summary>
    [Fact]
    public void TheVipDeskIsLast()
    {
        VipCallTaker vip = new VipCallTaker(null);
        Assert.Null(vip.Next);
    }

    /// <summary>Main prints each customer reaching the first desk that serves them, one blank line per customer.</summary>
    [Fact]
    public void MainOutput()
    {
        Assert.Equal(new[]
        {
            "StandardCallTaker received a customer.",
            "Standard customer: asking a question.",
            "Answer: Here is your answer!",
            "",
            "StandardCallTaker received a customer.",
            "GoldCallTaker received a customer.",
            "Gold customer: asking a GOLD question.",
            "Answer: Here is your GOLD answer!",
            "",
            "StandardCallTaker received a customer.",
            "GoldCallTaker received a customer.",
            "VipCallTaker received a customer.",
            "VIP customer: asking a VIP question.",
            "Answer: Here is your VIP answer!",
            ""
        }, By(Main.Run));
    }

    /// <summary>VipFirstMain prints the VIP desk answering all three customers.</summary>
    [Fact]
    public void VipFirstMainOutput()
    {
        Assert.Equal(new[]
        {
            "VipCallTaker received a customer.",
            "Standard customer: asking a question.",
            "Answer: Here is your VIP answer!",
            "",
            "VipCallTaker received a customer.",
            "Gold customer: asking a GOLD question.",
            "Answer: Here is your VIP answer!",
            "",
            "VipCallTaker received a customer.",
            "VIP customer: asking a VIP question.",
            "Answer: Here is your VIP answer!",
            ""
        }, By(VipFirstMain.Run));
    }
}
