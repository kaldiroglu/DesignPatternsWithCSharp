namespace dev.kaldiroglu.ChainOfResponsibility.CallCenter;

public class VipCallTaker : AbstractCallTaker
{
    public VipCallTaker(ICallTaker? next) : base(next)
    {
    }

    public override void Answer(ICustomer customer)
    {
        Console.WriteLine("VipCallTaker received a customer.");
        if (customer is VipCustomer)
        {
            customer.AskAQuestion();
            customer.ReceiveAnswer("Here is your VIP answer!");
        }
        else
        {
            PassOn(customer);
        }
        Console.WriteLine();
    }
}
