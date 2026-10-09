namespace dev.kaldiroglu.ChainOfResponsibility.Tests.CallCenter;

using dev.kaldiroglu.ChainOfResponsibility.CallCenter;
using Xunit;
using static dev.kaldiroglu.ChainOfResponsibility.Tests.Printed;

/// <summary>
/// The call center: standard, then gold, then VIP. The chain is built here as
/// Test.CreateCallTakers builds it, without the random customers.
/// </summary>
public class CallCenterTest
{
    private static ICallTaker StandardDesk()
    {
        VipCallTaker vip = new VipCallTaker(null);
        GoldCallTaker gold = new GoldCallTaker(vip);
        return new StandardCallTaker(gold);
    }

    private static List<string> DesksAndAnswers(ICustomer customer)
    {
        ICallTaker first = StandardDesk();
        return By(() => first.Answer(customer))
            .Where(line => line.EndsWith("received a customer.", StringComparison.Ordinal)
                           || line.StartsWith("Answer:", StringComparison.Ordinal))
            .ToList();
    }

    /// <summary>A standard customer is answered by the standard desk.</summary>
    [Fact]
    public void Standard()
    {
        Assert.Equal(new[]
        {
            "StandardCallTaker received a customer.",
            "Answer: Here is your answer!"
        }, DesksAndAnswers(new StandardCustomer()));
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
        }, DesksAndAnswers(new GoldCustomer()));
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
        }, DesksAndAnswers(new VipCustomer()));
    }

    /// <summary>The VIP desk is the end of the chain and has no next desk.</summary>
    [Fact]
    public void TheVipDeskIsLast()
    {
        VipCallTaker vip = new VipCallTaker(null);
        Assert.Null(vip.Next);
    }
}
