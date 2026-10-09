namespace dev.kaldiroglu.Command.Tests.Lender.Problem2;

// The usings sit inside the namespace, so that a name such as Lender finds the type before
// it finds the test namespace of the same name.
using dev.kaldiroglu.Command.Lender.Problem2;
using Xunit;

/// <summary>Step two: the lender knows a borrower interface. Ported from the Java <c>Problem2Test</c>.</summary>
public class Problem2Tests
{
    /// <summary>
    /// A borrower written in this test. Java uses a lambda; a C# lambda cannot implement an
    /// interface, so it is a small class.
    /// </summary>
    private sealed class Recorder(List<int> received) : IBorrower
    {
        public void Borrow(int money) => received.Add(money);
    }

    [Fact(DisplayName = "lend takes the Borrower interface")]
    public void LendNamesTheInterface()
    {
        Assert.Equal([typeof(IBorrower), typeof(int)], Printed.LendParameters(typeof(Lender)));
        Assert.True(typeof(IBorrower).IsInterface);
    }

    [Fact(DisplayName = "any borrower works, even one written in this test")]
    public void AnyBorrowerWorks()
    {
        var received = new List<int>();
        new Lender().Lend(new Recorder(received), 750);
        Assert.Equal([750], received);
    }

    [Fact(DisplayName = "the two borrowers spend the money in two ways")]
    public void TwoBorrowers()
    {
        var lender = new Lender();
        Assert.Equal(
            ["Borrowing 1000 and spending for family!", "Borrowing 2000 and spending for school!"],
            Printed.By(() =>
            {
                lender.Lend(new ConcreteBorrower1(), 1000);
                lender.Lend(new ConcreteBorrower2(), 2000);
            }));
    }

    [Fact(DisplayName = "Main prints two loans")]
    public void MainOutput()
    {
        Assert.Equal(
            ["Borrowing 1000 and spending for family!", "Borrowing 2000 and spending for school!"],
            Printed.By(Main.Run));
    }
}
