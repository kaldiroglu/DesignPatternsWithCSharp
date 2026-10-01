namespace dev.kaldiroglu.Command.Hw.Kitchen;

/// <summary>The <b>Receiver</b>: it cooks, in the order it is asked.</summary>
public sealed class Kitchen
{
    private readonly List<string> _cooked = [];

    public void Cook(string dish, int table) => _cooked.Add(dish + " for table " + table);

    public IReadOnlyList<string> Cooked() => _cooked.ToList().AsReadOnly();
}
