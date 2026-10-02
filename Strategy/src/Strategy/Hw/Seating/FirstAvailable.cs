namespace dev.kaldiroglu.Strategy.Hw.Seating;

/// <summary>Whatever is free, in order. The cheapest fare gets this.</summary>
public sealed class FirstAvailable : ISeatingPolicy
{
    public string Name => "FIRST_AVAILABLE";

    public IReadOnlyList<string> Allocate(SeatPlan plan, int partySize)
    {
        var free = plan.Free();
        return free.Count < partySize ? [] : [.. free.Take(partySize)];
    }
}
