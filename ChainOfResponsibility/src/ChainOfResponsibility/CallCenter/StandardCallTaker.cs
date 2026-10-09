namespace dev.kaldiroglu.ChainOfResponsibility.CallCenter;

public class StandardCallTaker : AbstractCallTaker
{
    public StandardCallTaker(ICallTaker? next) : base(next)
    {
    }

    public override void Answer(ICustomer customer)
    {
        Console.WriteLine("StandardCallTaker received a customer.");
        if (customer is StandardCustomer)
        {
            customer.AskAQuestion();
            customer.ReceiveAnswer("Here is your answer!");
        }
        else
        {
            next!.Answer(customer);
        }
        Console.WriteLine();
    }
}
