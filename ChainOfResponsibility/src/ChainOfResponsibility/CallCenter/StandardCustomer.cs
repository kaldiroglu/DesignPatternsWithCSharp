namespace dev.kaldiroglu.ChainOfResponsibility.CallCenter;

public class StandardCustomer : AbstractCustomer
{
    public override void AskAQuestion()
    {
        Console.WriteLine("Standard customer: asking a question.");
    }
}
