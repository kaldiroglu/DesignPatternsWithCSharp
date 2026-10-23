using System.Globalization;

namespace dev.kaldiroglu.Observer.Payment;

public class Invoice : Observable
{
    private static int count;
    private readonly int no;
    private double balance;

    public Invoice(double balance)
    {
        no = ++count;
        this.balance = balance;
    }

    public void PayBalance(double amount)
    {
        Console.WriteLine("Some payment made.");
        balance = balance - amount;
        SetChanged();
        NotifyObservers();
    }

    public override string ToString()
    {
        return "Invoice [balance=" + FormatDouble(balance) + ", no=" + no + "]";
    }

    /// <summary>
    /// Prints a <c>double</c> so that a whole number keeps <c>.0</c>: 5000 prints as
    /// <c>5000.0</c>.
    /// </summary>
    private static string FormatDouble(double value) =>
        value % 1 == 0
            ? value.ToString("0.0", CultureInfo.InvariantCulture)
            : value.ToString("R", CultureInfo.InvariantCulture);
}
