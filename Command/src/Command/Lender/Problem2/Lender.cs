namespace dev.kaldiroglu.Command.Lender.Problem2;

public class Lender
{
    public void Lend(IBorrower borrower, int money)
    {
        borrower.Borrow(money);
    }
}
