namespace dev.kaldiroglu.Strategy.Hw.LateFee;

/// <summary>
/// One overdue item.
/// </summary>
/// <param name="Title">what was borrowed</param>
/// <param name="DaysLate">how many days past the due date</param>
/// <param name="DailyRate">the base charge per day, in minor units</param>
public sealed record Loan(string Title, int DaysLate, int DailyRate)
{
    public int DaysLate { get; } = DaysLate >= 0
        ? DaysLate
        : throw new ArgumentException("an item cannot be returned before it is due", nameof(DaysLate));
}
