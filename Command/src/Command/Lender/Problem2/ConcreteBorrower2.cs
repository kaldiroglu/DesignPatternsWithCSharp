namespace dev.kaldiroglu.Command.Lender.Problem2;

public class ConcreteBorrower2 : IBorrower
{
    public void Borrow(int money)
    {
        Console.WriteLine("Borrowing " + money + " and spending for school!");
    }
}
