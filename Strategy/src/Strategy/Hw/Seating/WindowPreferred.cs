namespace dev.kaldiroglu.Strategy.Hw.Seating;

/// <summary>Windows first, then anything. What a frequent flyer is given.</summary>
public sealed class WindowPreferred : ISeatingPolicy
{
    public string Name => "WINDOW_PREFERRED";

    public IReadOnlyList<string> Allocate(SeatPlan plan, int partySize)
    {
        var free = plan.Free();
        if (free.Count < partySize)
        {
            return [];
        }
        var chosen = new List<string>(free.Where(plan.IsWindow));
        chosen.AddRange(free.Where(seat => !plan.IsWindow(seat)));
        return [.. chosen.Take(partySize)];
    }
}
