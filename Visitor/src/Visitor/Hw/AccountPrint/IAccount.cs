namespace dev.kaldiroglu.Visitor.Hw.AccountPrint;

/// <summary>
/// Homework 1: print every account of a bank to an output that is passed in.
/// <para>
/// The accounts do not print themselves. Each printer is a visitor, and it is given the
/// output — the console, a file, a string — when it is created. A new format is a new
/// visitor; the account classes do not change.
/// </para>
/// </summary>
public interface IAccount
{
    string Owner { get; }

    void Accept(IAccountVisitor visitor);
}
