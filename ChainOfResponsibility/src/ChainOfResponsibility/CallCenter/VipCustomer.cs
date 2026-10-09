namespace dev.kaldiroglu.ChainOfResponsibility.CallCenter;

public class VipCustomer : AbstractCustomer
{
    public override void AskAQuestion()
    {
        AskAVipQuestion();
    }

    private void AskAVipQuestion()
    {
        Console.WriteLine("\n*** Vip Customer: Asking a VIP question!");
    }
}
