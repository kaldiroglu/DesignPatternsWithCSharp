using System.Globalization;

namespace dev.kaldiroglu.Memento.Hw.Rollback;

/// <summary>Homework 2: the <b>Originator</b>. An account that cannot go below zero.</summary>
public sealed class Account
{
    /// <summary>The <b>Memento</b>, as the batch sees it: no members.</summary>
    public interface ISaved
    {
    }

    /// <summary>The balance at one moment. The class is private, so only the account can read it.</summary>
    private sealed class Saved : ISaved
    {
        public int Balance { get; }

        public Saved(int balance)
        {
            Balance = balance;
        }
    }

    private readonly string owner;
    private int balance;

    public Account(string owner, int balance)
    {
        this.owner = owner;
        this.balance = balance;
    }

    public void Withdraw(int amount)
    {
        if (amount > balance)
        {
            throw new InvalidOperationException(
                string.Create(CultureInfo.InvariantCulture, $"{owner} cannot pay {amount}"));
        }
        balance -= amount;
    }

    public void Deposit(int amount)
    {
        balance += amount;
    }

    public ISaved Save()
    {
        return new Saved(balance);
    }

    /// <summary>A memento of any other class throws <see cref="InvalidCastException"/>.</summary>
    public void Restore(ISaved saved)
    {
        balance = ((Saved)saved).Balance;
    }

    public override string ToString() =>
        string.Create(CultureInfo.InvariantCulture, $"{owner} {balance}");
}
