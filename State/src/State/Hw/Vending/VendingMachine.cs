namespace dev.kaldiroglu.State.Hw.Vending;

/// <summary>
/// Homework 3: a vending machine with a coin slot and a stock of drinks.
/// <para>
/// Three states: waiting for a coin, has a coin, sold out. The homework question was what
/// happens to a coin put into a sold-out machine. Here it is returned at once: the sold-out
/// state never takes it.
/// </para>
/// </summary>
public sealed class VendingMachine
{
    private IVendingState state;
    private int stock;
    private readonly List<string> log = [];

    public VendingMachine(int stock)
    {
        this.stock = stock;
        state = stock > 0 ? new WaitingForCoin() : new SoldOut();
    }

    public void InsertCoin()
    {
        state = state.InsertCoin(this);
    }

    public void PressButton()
    {
        state = state.PressButton(this);
    }

    public void Refill(int drinks)
    {
        stock += drinks;
        state = state.Refilled(this);
    }

    /// <summary>The name of the current state's class, as Java's <c>getSimpleName()</c> gives it.</summary>
    public string State => state.GetType().Name;

    internal int Stock => stock;

    internal void TakeOneDrink()
    {
        stock--;
    }

    internal void Record(string line)
    {
        log.Add(line);
    }

    public IReadOnlyList<string> Log => log.ToList().AsReadOnly();
}
