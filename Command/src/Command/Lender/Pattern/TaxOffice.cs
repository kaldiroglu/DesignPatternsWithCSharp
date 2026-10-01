namespace dev.kaldiroglu.Command.Lender.Pattern;

public class TaxOffice : ICommand
{
    public void Execute(int money)
    {
        Console.WriteLine("Receiving for the tax payment!");
    }
}
