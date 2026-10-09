namespace dev.kaldiroglu.Command.Tests.Lender.Pattern;

// The usings sit inside the namespace, so that a name such as Lender finds the type before
// it finds the test namespace of the same name.
using System.Reflection;
using dev.kaldiroglu.Command.Lender.Pattern;
using Xunit;

/// <summary>Step three: the lender runs any command. Ported from the Java <c>PatternTest</c>.</summary>
public class PatternTests
{
    /// <summary>
    /// A command written in this test. Java uses a method reference; a C# lambda cannot
    /// implement an interface, so it is a small class.
    /// </summary>
    private sealed class Recorder(List<int> received) : ICommand
    {
        public void Execute(int money) => received.Add(money);
    }

    [Fact(DisplayName = "lend takes a Command, and the lender names no concrete command")]
    public void LendNamesOnlyCommand()
    {
        Assert.Equal([typeof(ICommand), typeof(int)], Printed.LendParameters(typeof(Lender)));
        var methods = typeof(Lender).GetMethods(BindingFlags.Instance | BindingFlags.Static
                                                | BindingFlags.Public | BindingFlags.NonPublic
                                                | BindingFlags.DeclaredOnly);
        foreach (var method in methods)
        {
            foreach (var parameter in method.GetParameters())
            {
                Assert.NotEqual(typeof(Borrower), parameter.ParameterType);
                Assert.NotEqual(typeof(TaxOffice), parameter.ParameterType);
            }
        }
    }

    [Fact(DisplayName = "the money reaches the command at execute time")]
    public void TheAmountArrivesAtExecute()
    {
        var received = new List<int>();
        ICommand recorder = new Recorder(received);
        var lender = new Lender();
        lender.Lend(recorder, 1000);
        lender.Lend(recorder, 2000);
        Assert.Equal([1000, 2000], received);
    }

    [Fact(DisplayName = "a borrower and a tax office are both commands")]
    public void TwoCommands()
    {
        var lender = new Lender();
        Assert.Equal(
            ["Borrowing 1000 and spending for family!", "Receiving for the tax payment!"],
            Printed.By(() =>
            {
                lender.Lend(new Borrower(), 1000);
                lender.Lend(new TaxOffice(), 2000);
            }));
    }

    [Fact(DisplayName = "the tax office does not use the amount it is given")]
    public void TheTaxOfficeIgnoresTheAmount()
    {
        var lines = Printed.By(() => new TaxOffice().Execute(2000));
        Assert.Single(lines);
        Assert.DoesNotContain("2000", lines[0]);
    }

    [Fact(DisplayName = "Main prints a loan and a tax payment")]
    public void MainOutput()
    {
        Assert.Equal(
            ["Borrowing 1000 and spending for family!", "Receiving for the tax payment!"],
            Printed.By(Main.Run));
    }
}
