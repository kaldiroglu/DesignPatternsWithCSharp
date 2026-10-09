namespace dev.kaldiroglu.Command.Tests.Lender.Lambda;

using dev.kaldiroglu.Command.Lender.Lambda;
using Xunit;

/// <summary>The lender with the command as a function. Ported from the Java <c>LambdaTest</c>.</summary>
public class LambdaTests
{
    [Fact(DisplayName = "lend takes an Action<int>, a function from an amount to nothing")]
    public void LendTakesAFunction()
    {
        Assert.Equal([typeof(Action<int>), typeof(int)], Printed.LendParameters(typeof(Lender)));
    }

    [Fact(DisplayName = "a lambda receives the money at the moment of lending")]
    public void ALambdaIsTheCommand()
    {
        var received = new List<int>();
        var lender = new Lender();
        lender.Lend(received.Add, 1000);
        lender.Lend(received.Add, 2000);
        Assert.Equal([1000, 2000], received);
    }

    [Fact(DisplayName = "Main prints the borrower's line, and the tax office's line with the amount it received")]
    public void MainPrintsTheTwoLines()
    {
        Assert.Equal(["Borrowing 1000 and spending for family!", "Receiving for the tax payment: 2000"],
            Printed.By(Main.Run));
    }
}
