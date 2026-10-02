using System.Globalization;

namespace dev.kaldiroglu.Strategy.Hw.Seating;

/// <summary>
/// The cabin: which seats exist, which are free, and what each one is like.
/// </summary>
/// <param name="Rows">how many rows</param>
/// <param name="PerRow">seats per row, lettered from A</param>
/// <param name="Taken">seats already allocated</param>
public sealed record SeatPlan(int Rows, int PerRow, IReadOnlyList<string> Taken)
{
    public IReadOnlyList<string> Taken { get; } = [.. Taken];

    public static SeatPlan Empty(int rows, int perRow) => new(rows, perRow, []);

    public IReadOnlyList<string> Free() => [.. All().Where(seat => !Taken.Contains(seat))];

    public IReadOnlyList<string> All() =>
        [.. Enumerable.Range(1, Rows)
            .SelectMany(row => Enumerable.Range(0, PerRow)
                .Select(seat => row.ToString(CultureInfo.InvariantCulture) + (char)('A' + seat)))];

    /// <summary>A window seat is the first or last letter in its row.</summary>
    public bool IsWindow(string seat)
    {
        var letter = seat[^1];
        return letter == 'A' || letter == (char)('A' + PerRow - 1);
    }

    public int RowOf(string seat) => int.Parse(seat[..^1], CultureInfo.InvariantCulture);

    public SeatPlan WithTaken(IReadOnlyList<string> seats)
    {
        var now = new List<string>(Taken);
        now.AddRange(seats);
        return new SeatPlan(Rows, PerRow, now);
    }
}
