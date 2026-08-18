namespace dev.kaldiroglu.Bridge.Hw.StatementRun;

/// <summary>A RefinedAbstraction: what moved, and where it left the balance.</summary>
public sealed class AccountStatement : Document
{
    private readonly string _account;
    private readonly string _period;
    private readonly IReadOnlyList<string[]> _movements;
    private readonly string _closing;

    public AccountStatement(IMedium medium, string account, string period,
        IReadOnlyList<string[]> movements, string closing) : base(medium)
    {
        _account = account;
        _period = period;
        _movements = movements.ToList();
        _closing = closing;
    }

    protected override void Body()
    {
        Medium.Heading(1, "Account statement");
        Medium.Field("Account", _account);
        Medium.Field("Period", _period);
        Medium.Heading(2, "Movements");
        foreach (var movement in _movements)
        {
            Medium.Row(movement);
        }

        Medium.Total("Closing balance", _closing);
    }
}
