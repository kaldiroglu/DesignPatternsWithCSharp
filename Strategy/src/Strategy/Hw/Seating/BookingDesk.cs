namespace dev.kaldiroglu.Strategy.Hw.Seating;

/// <summary>The Context: holds a policy and seats a booking with it.</summary>
public sealed class BookingDesk
{
    private ISeatingPolicy _policy;

    public BookingDesk(ISeatingPolicy policy)
    {
        ArgumentNullException.ThrowIfNull(policy);
        _policy = policy;
    }

    public void SetPolicy(ISeatingPolicy policy)
    {
        ArgumentNullException.ThrowIfNull(policy);
        _policy = policy;
    }

    public string PolicyName => _policy.Name;

    public IReadOnlyList<string> Seat(SeatPlan plan, int partySize) => _policy.Allocate(plan, partySize);
}
