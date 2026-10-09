namespace dev.kaldiroglu.Command.Tests.Account.Lambda;

using dev.kaldiroglu.Command.Account.Domain;
using dev.kaldiroglu.Command.Account.Solution;
using Xunit;
using static dev.kaldiroglu.Command.Account.Lambda.Transactions;
using LambdaMain = dev.kaldiroglu.Command.Account.Lambda.Main;
using SolutionMain = dev.kaldiroglu.Command.Account.Solution.Main;

/// <summary>
/// The account's transactions written as lambdas, against the classes of <c>Solution</c>.
/// Ported from the Java <c>LambdaTransactionTest</c>.
/// </summary>
public class LambdaTransactionTests
{
    private static Money Lira(string amount) => Money.Of(amount);

    [Fact(DisplayName = "Main prints the same lines as the version with transaction classes")]
    public void SameOutputAsTheClasses()
    {
        Assert.Equal(Printed.By(SolutionMain.Run), Printed.By(LambdaMain.Run));
    }

    [Fact(DisplayName = "each lambda transaction writes the same journal line as its class")]
    public void SameDescriptions()
    {
        var deniz = new dev.kaldiroglu.Command.Account.Domain.Account("Deniz", Lira("1000.00"));
        var emre = new dev.kaldiroglu.Command.Account.Domain.Account("Emre", Lira("0.00"));
        Assert.Equal(new dev.kaldiroglu.Command.Account.Solution.Deposit(deniz, Lira("5.00")).Description,
            Deposit(deniz, Lira("5.00")).Description);
        Assert.Equal(new dev.kaldiroglu.Command.Account.Solution.Withdraw(deniz, Lira("5.00")).Description,
            Withdraw(deniz, Lira("5.00")).Description);
        Assert.Equal(new dev.kaldiroglu.Command.Account.Solution.Transfer(deniz, emre, Lira("300.00")).Description,
            Transfer(deniz, emre, Lira("300.00")).Description);
        Assert.Equal(new dev.kaldiroglu.Command.Account.Solution.CloseOut(deniz).Description, CloseOut(deniz).Description);
    }

    [Fact(DisplayName = "a close-out remembers what it took, in a variable the lambdas share")]
    public void CloseOutRemembers()
    {
        var deniz = new dev.kaldiroglu.Command.Account.Domain.Account("Deniz", Lira("750.00"));
        var teller = new Teller();
        teller.Perform(CloseOut(deniz));
        Assert.Equal(Lira("0.00"), deniz.Balance);
        Assert.Equal("close out Deniz, paid 750.00", teller.Journal()[^1]);
        teller.Undo();
        Assert.Equal(Lira("750.00"), deniz.Balance);
    }

    [Fact(DisplayName = "a transfer that cannot be paid changes nothing and is not recorded")]
    public void ATransferIsAllOrNothing()
    {
        var deniz = new dev.kaldiroglu.Command.Account.Domain.Account("Deniz", Lira("100.00"));
        var emre = new dev.kaldiroglu.Command.Account.Domain.Account("Emre", Lira("0.00"));
        var teller = new Teller();
        Assert.Throws<InsufficientFundsException>(() => teller.Perform(Transfer(deniz, emre, Lira("300.00"))));
        Assert.Equal(Lira("100.00"), deniz.Balance);
        Assert.Equal(Lira("0.00"), emre.Balance);
        Assert.Empty(teller.Journal());
    }

    [Fact(DisplayName = "undo and redo work on lambda transactions as on classes")]
    public void UndoAndRedo()
    {
        var deniz = new dev.kaldiroglu.Command.Account.Domain.Account("Deniz", Lira("1000.00"));
        var teller = new Teller();
        teller.Perform(Deposit(deniz, Lira("500.00")));
        teller.Perform(Withdraw(deniz, Lira("200.00")));
        teller.Undo();
        teller.Undo();
        Assert.Equal(Lira("1000.00"), deniz.Balance);
        teller.Redo();
        Assert.Equal(Lira("1500.00"), deniz.Balance);
    }
}
