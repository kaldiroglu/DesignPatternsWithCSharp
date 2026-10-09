namespace dev.kaldiroglu.Memento.Hw.Rollback;

/// <summary>
/// The <b>Caretaker</b>: runs a list of transfers as one unit.
/// <para>
/// Before the first transfer it takes a memento of every account. If any transfer fails, it
/// gives every account its memento back, so no half-done batch remains. It does not need to
/// know how to reverse each transfer.
/// </para>
/// </summary>
public sealed class Batch
{
    public sealed record Transfer(Account From, Account To, int Amount);

    private readonly List<Transfer> transfers = new List<Transfer>();

    public void Add(Account from, Account to, int amount)
    {
        transfers.Add(new Transfer(from, to, amount));
    }

    /// <summary>Returns "done", or why it was rolled back.</summary>
    public string Run(IReadOnlyList<Account> accounts)
    {
        // Keeps the order in which the accounts were given, as Java's LinkedHashMap does.
        var before = new OrderedDictionary<Account, Account.ISaved>();
        foreach (Account account in accounts)
        {
            before[account] = account.Save();
        }
        try
        {
            foreach (Transfer t in transfers)
            {
                t.From.Withdraw(t.Amount);
                t.To.Deposit(t.Amount);
            }
            return "done";
        }
        catch (InvalidOperationException e)
        {
            foreach (var (account, saved) in before)
            {
                account.Restore(saved);
            }
            return "rolled back: " + e.Message;
        }
    }
}
