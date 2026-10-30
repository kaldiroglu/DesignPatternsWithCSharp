using System.Globalization;

namespace dev.kaldiroglu.Observer.Payment;

/// <summary>
/// An invoice that uses .NET's own <see cref="IObservable{T}"/>: it keeps the list of
/// observers, and <see cref="Subscribe"/> returns the object that ends a subscription.
/// Observers are told first subscribed first; the interface does not specify an order.
/// </summary>
public class Invoice : IObservable<Invoice>
{
    private static int count;
    private readonly List<IObserver<Invoice>> observers = [];
    private readonly int no;
    private double balance;

    public Invoice(double balance)
    {
        no = ++count;
        this.balance = balance;
    }

    public void PayBalance(double amount)
    {
        Console.WriteLine("Some payment is made.");
        balance = balance - amount;
        foreach (IObserver<Invoice> observer in observers.ToArray())
        {
            observer.OnNext(this);
        }
    }

    public IDisposable Subscribe(IObserver<Invoice> observer)
    {
        ArgumentNullException.ThrowIfNull(observer);
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
        return new Subscription(observers, observer);
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

    private sealed class Subscription(List<IObserver<Invoice>> observers, IObserver<Invoice> observer)
        : IDisposable
    {
        public void Dispose() => observers.Remove(observer);
    }
}
