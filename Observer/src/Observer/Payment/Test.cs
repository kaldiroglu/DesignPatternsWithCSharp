namespace dev.kaldiroglu.Observer.Payment;

/// <summary>
/// An invoice with two observers. The boss is added first, but the accountant is told
/// first, as with the JDK's <c>Observable</c>. After the first payment the boss is removed.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>; its commented-out lines are not ported. Run it with
/// <c>dotnet run --project src/Observer.Demo -- payment</c>.
/// </remarks>
public static class Test
{
    public static void Run()
    {
        Invoice invoice1 = new Invoice(10_000);

        IObserver boss = new Boss();
        IObserver accountant = new Accountant();

        invoice1.AddObserver(boss);
        invoice1.AddObserver(accountant);

        invoice1.PayBalance(5000);
        invoice1.DeleteObserver(boss);

        invoice1.PayBalance(2000);
    }
}
