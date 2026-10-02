namespace dev.kaldiroglu.Strategy.Hw.Seating;

/// <summary>
/// The whole party in one row, or nothing.
/// <para>
/// The policy that shows why the interface returns a list and may return an empty one: this
/// algorithm can fail on a cabin the other two would happily seat, and that is a legitimate
/// answer rather than an exception.
/// </para>
/// </summary>
public sealed class KeepTogether : ISeatingPolicy
{
    public string Name => "KEEP_TOGETHER";

    public IReadOnlyList<string> Allocate(SeatPlan plan, int partySize)
    {
        for (var row = 1; row <= plan.Rows; row++)
        {
            var thisRow = row;
            var inRow = plan.Free().Where(seat => plan.RowOf(seat) == thisRow).ToList();
            if (inRow.Count >= partySize)
            {
                return [.. inRow.Take(partySize)];
            }
        }
        return [];
    }
}
