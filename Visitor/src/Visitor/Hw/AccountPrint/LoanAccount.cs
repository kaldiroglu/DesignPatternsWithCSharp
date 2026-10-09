namespace dev.kaldiroglu.Visitor.Hw.AccountPrint;

public sealed record LoanAccount(string Owner, int Debt, int MonthlyPayment) : IAccount
{
    public void Accept(IAccountVisitor visitor) => visitor.Visit(this);
}
