namespace dev.kaldiroglu.Observer.Payment;

public class Accountant : IObserver
{
    public void Update(Observable arg0, object? arg1)
    {
        Console.WriteLine("\nAccountant has received an update.");
        Invoice invoice = (Invoice)arg0;
        Console.WriteLine(invoice);
    }
}
