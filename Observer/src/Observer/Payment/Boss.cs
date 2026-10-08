namespace dev.kaldiroglu.Observer.Payment;

public class Boss : IObserver
{
    public void Update(Observable arg0, object? arg1)
    {
        Console.WriteLine("\nBoss has received an update.");
        Invoice invoice = (Invoice)arg0;
        Console.WriteLine(invoice);
    }
}
