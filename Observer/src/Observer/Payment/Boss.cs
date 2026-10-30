namespace dev.kaldiroglu.Observer.Payment;

public class Boss : IObserver<Invoice>
{
    public void OnNext(Invoice invoice)
    {
        Console.WriteLine("\nBoss has received an update.");
        Console.WriteLine(invoice);
    }

    public void OnError(Exception error)
    {
    }

    public void OnCompleted()
    {
    }
}
