namespace dev.kaldiroglu.Command.Account.Problem;

/// <summary>
/// The operations a teller can perform, as names.
/// <para>
/// Stages two and three remember an operation by writing down its name and its amount, and
/// then branch on the name to reverse it. Every constant here is a case in two switches.
/// </para>
/// </summary>
public enum Kind
{
    Deposit,
    Withdraw
}
