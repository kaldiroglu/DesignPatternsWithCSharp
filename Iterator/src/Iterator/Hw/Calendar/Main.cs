using System.Globalization;

namespace dev.kaldiroglu.Iterator.Hw.Calendar;

/// <summary>
/// Walks the business days of one week, skipping the weekend and a holiday, without storing a
/// list of dates.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Iterator.Demo -- hw-calendar</c>. The day of the week is
/// printed in capitals (<c>FRIDAY</c>).
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
