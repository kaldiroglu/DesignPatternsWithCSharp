namespace dev.kaldiroglu.Memento.Tests.Hw.Incremental;

using dev.kaldiroglu.Memento.Hw.Incremental;
using Xunit;

/// <summary>Homework 3: an incremental memento for a large sheet.</summary>
public class SheetTest
{
    /// <summary>An edit of two cells on a 10,000-cell sheet makes a memento with 2 entries.</summary>
    [Fact]
    public void TwoEntries()
    {
        Sheet sheet = new Sheet(100, 100);
        Assert.Equal(10_000, sheet.CellCount);
        Sheet.IChange change = sheet.Set(new Dictionary<string, int> { ["R1C1"] = 5, ["R2C2"] = 7 });
        Assert.Equal(2, change.Size);
    }

    /// <summary>Undoing the changes in reverse order gives the old values back.</summary>
    [Fact]
    public void UndoInReverseOrder()
    {
        Sheet sheet = new Sheet(100, 100);
        Sheet.IChange first = sheet.Set(new Dictionary<string, int> { ["R1C1"] = 5, ["R2C2"] = 7 });
        Sheet.IChange second = sheet.Set(new Dictionary<string, int> { ["R1C1"] = 9 });
        Assert.Equal(9, sheet.Get("R1C1"));
        sheet.Undo(second);
        Assert.Equal(5, sheet.Get("R1C1"));
        sheet.Undo(first);
        Assert.Equal(0, sheet.Get("R1C1"));
        Assert.Equal(0, sheet.Get("R2C2"));
    }
}
