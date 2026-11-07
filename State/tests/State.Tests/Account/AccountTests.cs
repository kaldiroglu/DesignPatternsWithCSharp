using System.Reflection;
using Xunit;

namespace dev.kaldiroglu.State.Tests.Account;

using Account = global::dev.kaldiroglu.State.Account.Account;
using Active = global::dev.kaldiroglu.State.Account.Active;
using AccountMain = global::dev.kaldiroglu.State.Account.Main;

/// <summary>The bank account: Active, Overdrawn, Frozen and Closed are status objects.</summary>
public class AccountTests
{
    [Fact]
    public void MainPrintsEveryStatus()
    {
        Assert.Equal(new[]
        {
            "Status: Active and balance: 1000",
            "Status: Active and balance: 2000",
            "Status: Active and balance: 1800",
            "Status: Overdrawn and balance: -700",
            "Status: Overdrawn and balance: -900",
            "Status: Overdrawn and balance: -1000",
            "Status: Frozen and balance: -1000",
            "In frozen state no transfer is allowed!",
            "Status: Active and balance: 1000",
            "Status: Active and balance: 0",
            "Status: Closed and balance: 0",
            "Account is closed!"
        }, Printed.By(AccountMain.Run));
    }

    [Fact]
    public void TheAccountKeepsNoFrozenFlag()
    {
        const BindingFlags all = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        Assert.Empty(typeof(Account).GetProperties(all).Where(p => p.PropertyType == typeof(bool)));
        Assert.Empty(typeof(Account).GetFields(all).Where(f => f.FieldType == typeof(bool)));
    }

    [Fact]
    public void AmountsMustBePositive()
    {
        Account? account = null;
        Printed.By(() => account = new Account(1000));
        Printed.By(() =>
        {
            Assert.ThrowsAny<ArgumentException>(() => account!.Deposit(-500));
            Assert.ThrowsAny<ArgumentException>(() => account!.Withdraw(-500));
            Assert.ThrowsAny<ArgumentException>(() => account!.Transfer(0));
        });
        Assert.Equal(1000, account!.Balance);
    }

    [Fact]
    public void ActiveRefusesTooLargeAWithdrawal()
    {
        Account? account = null;
        Printed.By(() => account = new Account(100));
        var printed = Printed.By(() => account!.Withdraw(5000));
        Assert.Equal("You can not withdraw money!", printed[^1]);
        Assert.Equal(100, account!.Balance);
        Assert.IsType<Active>(account.Status);
    }

    [Fact]
    public void ClosingTwice()
    {
        Account? account = null;
        Printed.By(() => account = new Account(0));
        Printed.By(account!.CloseAccount);
        Assert.Equal(new[] { "Account is already closed!" }, Printed.By(account.CloseAccount));
    }
}
