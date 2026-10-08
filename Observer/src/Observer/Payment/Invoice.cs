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
        return "Invoice [balance=" + JavaDouble(balance) + ", no=" + no + "]";
    }

    /// <summary>
    /// Prints a <c>double</c> the way Java does for these values: a whole number keeps
    /// <c>.0</c>, so 5000 prints as <c>5000.0</c>. Java switches to the E notation at ten
    /// million; this helper does not, and the example never gets near it.
    /// </summary>
    private static string JavaDouble(double value) =>
        value % 1 == 0
            ? value.ToString("0.0", CultureInfo.InvariantCulture)
            : value.ToString("R", CultureInfo.InvariantCulture);
}
