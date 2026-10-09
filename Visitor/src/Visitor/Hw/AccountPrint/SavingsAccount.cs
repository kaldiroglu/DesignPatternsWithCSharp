namespace dev.kaldiroglu.Visitor.Hw.AccountPrint;

public sealed record SavingsAccount(string Owner, int Balance, int InterestPercent) : IAccount
{
    public void Accept(IAccountVisitor visitor) => visitor.Visit(this);
}
