using System.Globalization;

namespace dev.kaldiroglu.Memento.Hw.Incremental;

/// <summary>
/// Homework 3: the <b>Originator</b>. A sheet with many cells.
/// <para>
/// Saving all ten thousand cells before every edit would be expensive. So a memento holds
/// only the old values of the cells one edit changed — an incremental memento, GoF
/// implementation issue 2 (storing incremental changes). This works because the mementos are
/// restored in the reverse order they were made.
/// </para>
/// </summary>
public sealed class Sheet
{
    /// <summary>
    /// The <b>Memento</b>, as the caller sees it: the old values of the changed cells only. The
    /// caller can ask how many cells it holds, and nothing else.
    /// </summary>
    public interface IChange
    {
        int Size { get; }
    }

    /// <summary>The old values. The class is private, so only the sheet can read them.</summary>
    private sealed class Change : IChange
    {
        public IReadOnlyDictionary<string, int> OldValues { get; }

        public Change(Dictionary<string, int> oldValues)
        {
            OldValues = new Dictionary<string, int>(oldValues);
        }

        public int Size => OldValues.Count;
    }

    private readonly Dictionary<string, int> cells = new Dictionary<string, int>();

    public Sheet(int rows, int columns)
    {
        for (int r = 1; r <= rows; r++)
        {
            for (int c = 1; c <= columns; c++)
            {
                cells[string.Create(CultureInfo.InvariantCulture, $"R{r}C{c}")] = 0;
            }
        }
    }

    /// <summary>Sets several cells in one edit and returns what is needed to undo exactly that edit.</summary>
    public IChange Set(IReadOnlyDictionary<string, int> newValues)
    {
        var old = new Dictionary<string, int>();
        foreach (var (cell, value) in newValues)
        {
            // Java's HashMap.put returns the old value; a C# dictionary does not, so the old
            // value is read first. A cell that does not exist throws KeyNotFoundException
            // here, before anything is set.
            old[cell] = cells[cell];
            cells[cell] = value;
        }
        return new Change(old);
    }

    /// <summary>A change of any other class throws <see cref="InvalidCastException"/>.</summary>
    public void Undo(IChange change)
    {
        foreach (var (cell, value) in ((Change)change).OldValues)
        {
            cells[cell] = value;
        }
    }

    public int Get(string cell)
    {
        return cells[cell];
    }

    public int CellCount => cells.Count;
}
