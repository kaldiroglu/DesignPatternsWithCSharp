namespace dev.kaldiroglu.Mediator.Hw.BookingForm;

/// <summary>
/// Books a room for six people. The small room disables Book and shows a warning; the
/// large room enables it. The form decides this, not the fields.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Mediator.Demo -- hw-bookingform</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        BookingForm form = new BookingForm();
        form.ChooseRoom("Small");
        form.ChooseDate("2026-10-12");
        form.SetAttendees(6);
        Console.WriteLine("Small room: Book enabled " + Show(form.BookEnabled) + ", warning '" + form.Warning + "'");
        form.ChooseRoom("Large");
        Console.WriteLine("Large room: Book enabled " + Show(form.BookEnabled) + ", warning '" + form.Warning + "'");
        form.ClickBook();
        Console.WriteLine("Booked: " + Show(form.Booked));
    }

    /// <summary>Prints a boolean the way Java does: <c>true</c> or <c>false</c>, not <c>True</c>.</summary>
    private static string Show(bool value) => value ? "true" : "false";

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
