namespace dev.kaldiroglu.Command.Tests.Account;

// The usings sit inside the namespace, so that the name Account finds the type before it
// finds the namespace of the same name.
using System.Text.RegularExpressions;
using dev.kaldiroglu.Command.Account.Domain;
using dev.kaldiroglu.Command.Account.Solution;
using Xunit;
using BankAccount = dev.kaldiroglu.Command.Account.Domain.Account;

/// <summary>
/// The same teller, with each request an object. Ported from the Java
/// <c>account.SolutionTest</c>.
/// </summary>
public class SolutionTests
{
    private const string Source = "Account/Solution/";

    private static Money Lira(string amount) => Money.Of(amount);

    [Fact(DisplayName = "the reversal, answered: one undo takes back the whole transfer")]
    public void ATransferIsUndoneAsOne()
    {
        var deniz = new BankAccount("Deniz", Lira("1000.00"));
        var emre = new BankAccount("Emre", Lira("0.00"));
        var teller = new Teller();

        teller.Perform(new Transfer(deniz, emre, Lira("300.00")));
        Assert.Equal(Lira("700.00"), deniz.Balance);
        Assert.Equal(Lira("300.00"), emre.Balance);

        teller.Undo();

        Assert.Equal(Lira("1000.00"), deniz.Balance);
        Assert.Equal(Lira("0.00"), emre.Balance);
        // One transfer, one line, one undo.
        Assert.Equal(["transfer 300.00 Deniz -> Emre", "undo transfer 300.00 Deniz -> Emre"],
            teller.Journal());
    }

    [Fact(DisplayName = "undo as far back as you like, and redo what you undid")]
    public void UndoAndRedo()
    {
        var deniz = new BankAccount("Deniz", Lira("1000.00"));
        var teller = new Teller();
        teller.Perform(new Deposit(deniz, Lira("500.00")));
        teller.Perform(new Withdraw(deniz, Lira("200.00")));

        teller.Undo();
        teller.Undo();
        Assert.Equal(Lira("1000.00"), deniz.Balance);
        Assert.False(teller.CanUndo);

        teller.Redo();
        Assert.Equal(Lira("1500.00"), deniz.Balance);
        Assert.True(teller.CanRedo);
    }

    [Fact(DisplayName = "a command remembers what it did: close out 750, undo, and 750 come back")]
    public void CloseOutRemembersWhatItTook()
    {
        var deniz = new BankAccount("Deniz", Lira("750.00"));
        var teller = new Teller();

        teller.Perform(new CloseOut(deniz));
        Assert.Equal(Lira("0.00"), deniz.Balance);

        teller.Undo();
        Assert.Equal(Lira("750.00"), deniz.Balance);
        Assert.Equal("close out Deniz, paid 750.00", teller.Journal()[0]);
    }

    [Fact(DisplayName = "a failed request never reaches the history, so it can never be undone into money")]
    public void AFailedWithdrawalIsNotRecorded()
    {
        var deniz = new BankAccount("Deniz", Lira("500.00"));
        var teller = new Teller();

        Assert.Throws<InsufficientFundsException>(
            () => teller.Perform(new Withdraw(deniz, Lira("600.00"))));
        teller.Undo();

        Assert.Equal(Lira("500.00"), deniz.Balance);
        Assert.Empty(teller.Journal());
    }

    [Fact(DisplayName = "a transfer that cannot be paid leaves both accounts as they were")]
    public void ATransferIsAllOrNothing()
    {
        var deniz = new BankAccount("Deniz", Lira("100.00"));
        var emre = new BankAccount("Emre", Lira("0.00"));

        Assert.Throws<InsufficientFundsException>(
            () => new Teller().Perform(new Transfer(deniz, emre, Lira("300.00"))));

        Assert.Equal(Lira("100.00"), deniz.Balance);
        Assert.Equal(Lira("0.00"), emre.Balance);
    }

    [Fact(DisplayName = "the teller names no operation: no switch, no case, no instanceof")]
    public void NoBranchInTheInvoker()
    {
        var code = ProblemTests.CodeOf(Source + "Teller.cs");

        Assert.Equal(0, SourceText.CountOf(code, "switch"));
        Assert.Equal(0, SourceText.CountOf(code, "case "));
        // Java looks for `instanceof`; the C# type tests are the keywords `is` and `as`.
        Assert.False(Regex.IsMatch(code, @"\bis\b"), "no type test with is");
        Assert.False(Regex.IsMatch(code, @"\bas\b"), "no type test with as");
        foreach (var operation in new[] { "Deposit", "Withdraw", "Transfer", "CloseOut" })
        {
            Assert.True(SourceText.CountOf(code, operation) == 0, operation + " is not named in the teller");
        }
    }

    /// <summary>
    /// A new operation, written in this test. Java writes it as an anonymous class; C# has no
    /// anonymous classes that implement an interface, so it is a nested class here.
    /// </summary>
    private sealed class MonthlyFee(BankAccount account) : ITransaction
    {
        private readonly Withdraw _charge = new(account, Money.Of("25.00"));

        public void Execute() => _charge.Execute();

        public void Undo() => _charge.Undo();

        public string Description => "monthly fee";
    }

    [Fact(DisplayName = "a new operation costs one class: a fee written in this test, undone like the rest")]
    public void ANewOperationCostsOneClass()
    {
        var deniz = new BankAccount("Deniz", Lira("1000.00"));
        var teller = new Teller();

        teller.Perform(new MonthlyFee(deniz));
        Assert.Equal(Lira("975.00"), deniz.Balance);

        teller.Undo();
        Assert.Equal(Lira("1000.00"), deniz.Balance);
        Assert.Equal("undo monthly fee", teller.Journal()[^1]);
    }

    [Fact(DisplayName = "standing orders: made in the morning, run at night, undoable tomorrow")]
    public void RequestsCanWait()
    {
        var deniz = new BankAccount("Deniz", Lira("1000.00"));
        var landlord = new BankAccount("Landlord", Lira("0.00"));
        var savings = new BankAccount("Savings", Lira("0.00"));
        var orders = new StandingOrders();

        orders.Schedule(new Transfer(deniz, landlord, Lira("400.00")));
        orders.Schedule(new Transfer(deniz, savings, Lira("100.00")));
        orders.Schedule(new Deposit(savings, Lira("5.00")));

        Assert.Equal(3, orders.Pending);
        Assert.Equal(Lira("1000.00"), deniz.Balance);   // nothing has happened yet

        var nightRun = new Teller();
        orders.RunThrough(nightRun);

        Assert.Equal(0, orders.Pending);
        Assert.Equal(Lira("500.00"), deniz.Balance);
        Assert.Equal(Lira("105.00"), savings.Balance);
        Assert.Equal(3, nightRun.Journal().Count);

        nightRun.Undo();
        Assert.Equal(Lira("100.00"), savings.Balance);   // tonight's last order, taken back
    }

    [Fact(DisplayName = "execute takes no arguments: the request was complete when it was made")]
    public void TheRequestIsComplete()
    {
        Assert.Empty(typeof(ITransaction).GetMethod("Execute")!.GetParameters());
        Assert.Empty(typeof(ITransaction).GetMethod("Undo")!.GetParameters());
        // Java counts execute, undo and description. In C# Description is a property, and its
        // getter get_Description is the third method.
        Assert.Equal(3, typeof(ITransaction).GetMethods().Length);
    }
}
