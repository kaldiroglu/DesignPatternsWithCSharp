namespace dev.kaldiroglu.ChainOfResponsibility.CallCenter;

public class StandardCallTaker : AbstractCallTaker
{
    public StandardCallTaker(ICallTaker? next) : base(next)
    {
    }

    public override void Answer(ICustomer customer)
    {
        Console.WriteLine("StandardCallTaker received a customer.");
        // NOTE: only a GoldCustomer is passed on. A VipCustomer is answered here, by the
        // standard desk, with "Here is your answer!". The Java has the same behavior.
        if (customer is GoldCustomer)
        {
            next!.Answer(customer);
        }
        else
        {
            customer.AskAQuestion();
            customer.ReceiveAnswer("Here is your answer!");
        }
        Console.WriteLine();
    }
}
