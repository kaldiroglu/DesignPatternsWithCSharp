namespace dev.kaldiroglu.ChainOfResponsibility.Tests.Hw.CashDispenser;

using dev.kaldiroglu.ChainOfResponsibility.Hw.CashDispenser;
using Xunit;

/// <summary>Homework 3: a cash machine with 200, 100, 50 and 20 notes.</summary>
public class NoteSlotTest
{
    private static NoteSlot Machine()
    {
        NoteSlot first = new NoteSlot(200);
        first.Then(new NoteSlot(100)).Then(new NoteSlot(50)).Then(new NoteSlot(20));
        return first;
    }

    /// <summary>Each slot pays what it can and passes the rest on.</summary>
    [Fact]
    public void EachSlotPaysItsPart()
    {
        Assert.Equal(new[] { "1 x 200", "1 x 100", "1 x 50", "1 x 20" }, Machine().Pay(370));
    }

    /// <summary>380 leaves 10 that nobody can pay.</summary>
    [Fact]
    public void ThreeHundredEighty()
    {
        Assert.Equal(new[] { "1 x 200", "1 x 100", "1 x 50", "1 x 20", "10 cannot be paid" },
            Machine().Pay(380));
    }

    /// <summary>260 leaves 10 as well, although 100 + 100 + 20 + 20 + 20 would pay it.</summary>
    [Fact]
    public void TwoHundredSixty()
    {
        Assert.Equal(new[] { "1 x 200", "1 x 50", "10 cannot be paid" }, Machine().Pay(260));
        Assert.Equal(260, 100 + 100 + 20 + 20 + 20);
        NoteSlot withoutTheTwoHundred = new NoteSlot(100);
        withoutTheTwoHundred.Then(new NoteSlot(20));
        // The split exists; the chain does not find it because each slot decides alone.
        Assert.Equal(new[] { "2 x 100", "3 x 20" }, withoutTheTwoHundred.Pay(260));
    }
}
