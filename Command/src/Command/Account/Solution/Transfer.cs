namespace dev.kaldiroglu.Command.Account.Solution;

using dev.kaldiroglu.Command.Account.Domain;

/// <summary>
/// A <b>ConcreteCommand</b> made of other commands: GoF's <c>MacroCommand</c>, with a bank's
/// name.
/// <para>
/// A transfer still reuses the withdrawal and the deposit — the reuse was never the mistake
/// in <c>Problem.Teller</c>. The mistake was that the teller recorded the two parts and
/// forgot the whole. Here the whole is one object, so it goes on the history once and comes
/// off it once: undo takes back both halves, in reverse order.
/// </para>
/// <para>
/// It is also all or nothing on the way in. If a later step fails, the steps already taken
/// are undone before the failure is passed on, so a half-made transfer is never left behind.
/// </para>
/// </summary>
public sealed class Transfer : ITransaction
{
    private readonly IReadOnlyList<ITransaction> _steps;

    public Transfer(Account from, Account to, Money amount)
    {
        _steps = [new Withdraw(from, amount), new Deposit(to, amount)];
        Description = "transfer " + amount + " " + from.Owner + " -> " + to.Owner;
    }

    public void Execute()
    {
        var taken = new List<ITransaction>();
        try
        {
            foreach (var step in _steps)
            {
                step.Execute();
                taken.Add(step);
            }
        }
        catch (Exception)
        {
            for (var i = taken.Count - 1; i >= 0; i--)
            {
                taken[i].Undo();
            }

            throw;
        }
    }

    public void Undo()
    {
        for (var i = _steps.Count - 1; i >= 0; i--)
        {
            _steps[i].Undo();
        }
    }

    public string Description { get; }
}
