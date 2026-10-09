namespace dev.kaldiroglu.ChainOfResponsibility.CallCenter;

public class VipCallTaker : AbstractCallTaker
{
    public VipCallTaker(ICallTaker? next) : base(next)
    {
    }

    public override void Answer(ICustomer customer)
    {
        Console.WriteLine("VipCallTaker received a customer.");
        customer.AskAQuestion();
        customer.ReceiveAnswer("Here is your VIP answer!");
        Console.WriteLine();
    }
}
