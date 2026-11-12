namespace dev.kaldiroglu.ChainOfResponsibility.CallCenter;

/// <summary>Serves VIP customers, and gold and standard customers too.</summary>
public class VipCallTaker : AbstractCallTaker
{
    public VipCallTaker(ICallTaker? next) : base(next)
    {
    }

    public override void Answer(ICustomer customer)
    {
        Console.WriteLine("VipCallTaker received a customer.");
        if (customer is VipCustomer or GoldCustomer or StandardCustomer)
        {
            customer.AskAQuestion();
            customer.ReceiveAnswer("Here is your VIP answer!");
        }
        else
        {
            PassOn(customer);
        }
    }
}
