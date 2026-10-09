using System.Globalization;

namespace dev.kaldiroglu.Iterator.Hw.Calendar;

/// <summary>
/// Walks the business days of one week, skipping the weekend and a holiday, without storing a
/// list of dates.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Iterator.Demo -- hw-calendar</c>. Java prints the day of the
/// week in capitals (<c>FRIDAY</c>), so the .NET name is upper-cased.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var holiday = new DateOnly(2026, 10, 29);
        var days = new BusinessDays(new DateOnly(2026, 10, 23), new DateOnly(2026, 10, 30), [holiday]);

        Console.WriteLine("Business days from 2026-10-23 to 2026-10-30, with "
            + Iso(holiday) + " a holiday:");
        foreach (var day in days)
        {
            Console.WriteLine("  " + Iso(day) + " " + day.DayOfWeek.ToString().ToUpperInvariant());
        }
    }

    private static string Iso(DateOnly day) => day.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
}
