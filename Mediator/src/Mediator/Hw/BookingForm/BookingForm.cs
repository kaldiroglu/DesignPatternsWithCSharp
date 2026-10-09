using System.Globalization;

namespace dev.kaldiroglu.Mediator.Hw.BookingForm;

/// <summary>
/// Homework 3: a meeting-room booking form, with the form itself as the <b>Mediator</b>.
/// <para>
/// Three fields depend on each other: the room has a capacity, the number of attendees must
/// fit in it, and Book is enabled only when a room and a date are chosen and the people fit.
/// Each field reports a change; the form decides what follows, in <see cref="Changed"/>. No
/// field knows another.
/// </para>
/// </summary>
public sealed class BookingForm
{
    private static readonly IReadOnlyDictionary<string, int> Capacity = new Dictionary<string, int>
    {
        ["Small"] = 4,
        ["Large"] = 12
    };

    private string room = "";
    private string date = "";
    private int attendees;
    private readonly List<string> booked = [];

    public void ChooseRoom(string room)
    {
        this.room = room;
        Changed();
    }

    public void ChooseDate(string date)
    {
        this.date = date;
        Changed();
    }

    public void SetAttendees(int attendees)
    {
        this.attendees = attendees;
        Changed();
    }

    public void ClickBook()
    {
        if (BookEnabled)
        {
            booked.Add(room + " on " + date + " for " + attendees.ToString(CultureInfo.InvariantCulture));
        }
    }

    /// <summary>Every rule of the form, in one place.</summary>
    private void Changed()
    {
        int capacity = Capacity.GetValueOrDefault(room, 0);
        bool fits = attendees > 0 && attendees <= capacity;
        Warning = room.Length > 0 && attendees > capacity
                ? room + " holds " + capacity.ToString(CultureInfo.InvariantCulture) + " people"
                : "";
        BookEnabled = room.Length > 0 && date.Length > 0 && fits;
    }

    public bool BookEnabled { get; private set; }

    public string Warning { get; private set; } = "";

    /// <summary>A copy of the bookings, as Java's <c>List.copyOf</c> gives.</summary>
    public IReadOnlyList<string> Booked => booked.ToList();
}
