namespace dev.kaldiroglu.ChainOfResponsibility.CallCenter;

public class GoldCustomer : AbstractCustomer
{
    public override void AskAQuestion()
    {
        Console.WriteLine("Gold customer: asking a GOLD question.");
    }
}
