namespace dev.kaldiroglu.Mediator.Tests.Hw.BankQueue;

using dev.kaldiroglu.Mediator.Hw.BankQueue;
using Xunit;
using static dev.kaldiroglu.Mediator.Tests.Fields;

/// <summary>Homework 1: the queue manager pairs customers and tellers in arrival order.</summary>
public class QueueManagerTest
{
    /// <summary>Customers take numbers, and free tellers call them in arrival order.</summary>
    [Fact]
    public void PairsInArrivalOrder()
    {
        QueueManager manager = new QueueManager();
        Teller tellerOne = new Teller("Teller 1", manager);
        Teller tellerTwo = new Teller("Teller 2", manager);
        new Customer("Elif", manager).Arrive();
        new Customer("Burak", manager).Arrive();
        tellerTwo.Free();
        tellerOne.Free();
        tellerOne.Free();
        new Customer("Mert", manager).Arrive();
        Assert.Equal(new[]
        {
            "Elif takes number 1",
            "Burak takes number 2",
            "Teller 2 calls number 1 (Elif)",
            "Teller 1 calls number 2 (Burak)",
            "Teller 1 waits for a customer",
            "Mert takes number 3",
            "Teller 1 calls number 3 (Mert)"
        }, manager.Log);
    }

    /// <summary>Customers and tellers never refer to each other.</summary>
    [Fact]
    public void NoColleagueKnowsAnother()
    {
        Assert.False(HoldsAny(typeof(Customer), [typeof(Teller)]));
        Assert.False(HoldsAny(typeof(Teller), [typeof(Customer)]));
    }
}
