namespace dev.kaldiroglu.Command.Tests.Lender.Problem1;

// The usings sit inside the namespace, so that a name such as Lender finds the type before
// it finds the test namespace of the same name.
using dev.kaldiroglu.Command.Lender.Problem1;
using Xunit;

/// <summary>Step one: the lender knows one borrower class. Ported from the Java <c>Problem1Test</c>.</summary>
public class Problem1Tests
{
    [Fact(DisplayName = "the lender passes the money to the borrower")]
    public void LendsToTheBorrower()
    {
        Assert.Equal(["Borrowing 1000 and spending for family!"],
            Printed.By(() => new Lender().Lend(new Borrower(), 1000)));
    }

    [Fact(DisplayName = "lend takes the concrete Borrower class, so no other borrower fits")]
    public void LendNamesTheConcreteClass()
    {
        Assert.Equal([typeof(Borrower), typeof(int)], Printed.LendParameters(typeof(Lender)));
        Assert.False(typeof(Borrower).IsInterface);
    }

    [Fact(DisplayName = "Main prints one loan")]
    public void MainOutput()
    {
        Assert.Equal(["Borrowing 1000 and spending for family!"], Printed.By(Main.Run));
    }
}
