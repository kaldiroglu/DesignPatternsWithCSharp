namespace dev.kaldiroglu.Strategy.Hw.Seating;

/// <summary>
/// The Strategy: how a booking of <c>partySize</c> people is seated.
/// <para>
/// Homework 1. Three policies are supplied; the exercise is a fourth, and the harder question
/// of whether the interface should have been about seats at all.
/// </para>
/// </summary>
public interface ISeatingPolicy
{
    string Name { get; }

    /// <summary>Seats for one booking, or an empty list when this policy cannot serve it.</summary>
    IReadOnlyList<string> Allocate(SeatPlan plan, int partySize);
}
