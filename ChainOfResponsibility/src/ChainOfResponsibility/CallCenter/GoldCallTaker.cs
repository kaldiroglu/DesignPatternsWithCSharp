namespace dev.kaldiroglu.ChainOfResponsibility.CallCenter;

public class GoldCallTaker : AbstractCallTaker
{
    public GoldCallTaker(ICallTaker? next) : base(next)
    {
    }

    public override void Answer(ICustomer customer)
    {
        Console.WriteLine("GoldCallTaker received a customer.");
        if (customer is VipCustomer)
        {
            next!.Answer(customer);
        }
        else
        {
            customer.AskAQuestion();
            customer.ReceiveAnswer("Here is your GOLD answer!");
        }
        Console.WriteLine();
    }
}
