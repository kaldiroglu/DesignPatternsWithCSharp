namespace dev.kaldiroglu.Command.Lender.Problem1;

public class Lender
{
    public void Lend(Borrower borrower, int money)
    {
        borrower.Borrow(money);
    }
}
