namespace dev.kaldiroglu.ChainOfResponsibility.CallCenter;

/// <summary>Serves gold customers, and standard customers too.</summary>
public class GoldCallTaker : AbstractCallTaker
{
    public GoldCallTaker(ICallTaker? next) : base(next)
    {
    }

    public override void Answer(ICustomer customer)
    {
        Console.WriteLine("GoldCallTaker received a customer.");
        if (customer is GoldCustomer or StandardCustomer)
        {
            customer.AskAQuestion();
            customer.ReceiveAnswer("Here is your GOLD answer!");
        }
        else
        {
            PassOn(customer);
        }
    }
}
