namespace dev.kaldiroglu.Command.Gof.Solution;

/// <summary>A list of menu items. Toolkit code: it knows labels and commands, and nothing else.</summary>
public sealed class Menu
{
    private readonly List<MenuItem> _items = [];

    public Menu Add(MenuItem item)
    {
        _items.Add(item);
        return this;
    }

    public void Click(string label)
    {
        var item = _items.FirstOrDefault(i => i.Label == label)
                   ?? throw new ArgumentException("no item labeled " + label);
        item.Clicked();
    }

    public IReadOnlyList<string> Labels() => _items.Select(item => item.Label).ToList().AsReadOnly();
}
