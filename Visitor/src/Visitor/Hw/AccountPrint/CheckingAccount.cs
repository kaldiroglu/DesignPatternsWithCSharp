namespace dev.kaldiroglu.Visitor.Hw.AccountPrint;

public sealed record CheckingAccount(string Owner, int Balance, int OverdraftLimit) : IAccount
{
    public void Accept(IAccountVisitor visitor) => visitor.Visit(this);
}
