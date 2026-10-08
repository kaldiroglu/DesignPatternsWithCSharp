using System.Collections;

namespace dev.kaldiroglu.Iterator.Hw.Calendar;

/// <summary>
/// Homework 2: the business days between two dates.
/// </summary>
/// <remarks>
/// There is no collection behind this iterator. It holds a date and computes the next one
/// when asked, skipping weekends and holidays. A year of business days is never stored. This
/// is the point of the exercise: an iterator hides how elements are produced, and "stored in
/// a list" is only one way.
/// </remarks>
public sealed class BusinessDays : IEnumerable<DateOnly>
{
    private readonly DateOnly _from;
    private readonly DateOnly _to;
    private readonly HashSet<DateOnly> _holidays;

    /// <summary>Every business day from <paramref name="from"/> up to and including <paramref name="to"/>.</summary>
    public BusinessDays(DateOnly from, DateOnly to, IEnumerable<DateOnly> holidays)
    {
        _from = from;
        _to = to;
        _holidays = [.. holidays];
    }

    public IEnumerator<DateOnly> GetEnumerator() => new Enumerator(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// The iterator. Java writes it as an anonymous class inside <c>iterator()</c>; C# has no
    /// anonymous classes, so it is a private nested class.
    /// </summary>
    private sealed class Enumerator : IEnumerator<DateOnly>
    {
        private readonly BusinessDays _days;
        private DateOnly _next;
        private DateOnly? _current;

        internal Enumerator(BusinessDays days)
        {
            _days = days;
            _next = days.FirstOnOrAfter(days._from);
        }

        public DateOnly Current =>
            _current ?? throw new InvalidOperationException("no current day: call MoveNext first");

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (_next > _days._to)
            {
                _current = null;
                return false;
            }

            _current = _next;
            _next = _days.FirstOnOrAfter(_next.AddDays(1));
            return true;
        }

        public void Reset()
        {
            _next = _days.FirstOnOrAfter(_days._from);
            _current = null;
        }

        public void Dispose()
        {
        }
    }

    private DateOnly FirstOnOrAfter(DateOnly day)
    {
        DateOnly candidate = day;
        while (IsWeekend(candidate) || _holidays.Contains(candidate))
        {
            candidate = candidate.AddDays(1);
        }

        return candidate;
    }

    private static bool IsWeekend(DateOnly day) =>
        day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday;
}
