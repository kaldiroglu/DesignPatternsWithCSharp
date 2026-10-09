namespace dev.kaldiroglu.ChainOfResponsibility.CallCenter;

public class StandardCustomer : AbstractCustomer
{
    public override void AskAQuestion()
    {
        AskAStandardQuestion();
    }

    private void AskAStandardQuestion()
    {
        Console.WriteLine("\nStandard Customer: Asking a question!");
    }
}
