namespace dev.kaldiroglu.ChainOfResponsibility.CallCenter;

public class VipCustomer : AbstractCustomer
{
    public override void AskAQuestion()
    {
        Console.WriteLine("VIP customer: asking a VIP question.");
    }
}
