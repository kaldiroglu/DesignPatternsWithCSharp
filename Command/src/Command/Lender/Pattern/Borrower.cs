namespace dev.kaldiroglu.Command.Lender.Pattern;

public class Borrower : ICommand
{
    public void Execute(int money)
    {
        Console.WriteLine("Borrowing " + money + " and spending for family!");
    }
}
