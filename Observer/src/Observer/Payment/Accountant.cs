namespace dev.kaldiroglu.Observer.Payment;

public class Accountant : IObserver<Invoice>
{
    public void OnNext(Invoice invoice)
    {
        Console.WriteLine("\nAccountant has received an update.");
        Console.WriteLine(invoice);
    }

    public void OnError(Exception error)
    {
    }

    public void OnCompleted()
    {
    }
}
