namespace dev.kaldiroglu.Observer.Payment;

/// <summary>
/// An invoice with two observers. The boss subscribes first and is told first. After the
/// first payment the boss ends its subscription.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Observer.Demo -- payment</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        Invoice invoice1 = new Invoice(10_000);

        IDisposable bossSubscription = invoice1.Subscribe(new Boss());
        invoice1.Subscribe(new Accountant());

        invoice1.PayBalance(5000);
        bossSubscription.Dispose();

        invoice1.PayBalance(2000);
    }
}
