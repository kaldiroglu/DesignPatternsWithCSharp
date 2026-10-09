namespace dev.kaldiroglu.Visitor.Hw.AccountPrint;

/// <summary>The <b>Visitor</b>: one method for each kind of account.</summary>
public interface IAccountVisitor
{
    void Visit(CheckingAccount account);

    void Visit(SavingsAccount account);

    void Visit(LoanAccount account);
}
