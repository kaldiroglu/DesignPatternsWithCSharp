namespace dev.kaldiroglu.ChainOfResponsibility.CallCenter;

public class GoldCustomer : AbstractCustomer
{
    public override void AskAQuestion()
    {
        AskAGoldQuestion();
    }

    private void AskAGoldQuestion()
    {
        Console.WriteLine("\n--- Gold Customer: Asking a GOLD question!");
    }
}
