using System.Globalization;

namespace dev.kaldiroglu.ChainOfResponsibility.Hw.CashDispenser;

/// <summary>
/// Homework 3: a cash machine. Each slot holds one kind of note, pays out as many as it
/// can, and passes the rest of the amount to the next slot.
/// <para>
/// Every link handles part of the request. What is left at the end of the chain cannot be
/// paid. Each slot decides alone, taking the largest notes first, so the chain can fail
/// where another split of notes would work: 260 leaves 10 after 200 and 50, although
/// 100 + 100 + 20 + 20 + 20 is 260.
/// </para>
/// </summary>
public sealed class NoteSlot
{
    private readonly int note;
    private NoteSlot? next;

    public NoteSlot(int note)
    {
        this.note = note;
    }

    public NoteSlot Then(NoteSlot next)
    {
        this.next = next;
        return next;
    }

    public IReadOnlyList<string> Pay(int amount)
    {
        List<string> paid = [];
        int count = amount / note;
        int rest = amount % note;
        if (count > 0)
        {
            paid.Add(Number(count) + " x " + Number(note));
        }
        if (rest == 0)
        {
            return paid;
        }
        if (next == null)
        {
            paid.Add(Number(rest) + " cannot be paid");
            return paid;
        }
        paid.AddRange(next.Pay(rest));
        return paid;
    }

    private static string Number(int value) => value.ToString(CultureInfo.InvariantCulture);
}
