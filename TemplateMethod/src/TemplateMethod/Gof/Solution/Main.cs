namespace dev.kaldiroglu.TemplateMethod.Gof.Solution;

/// <summary>
/// Opens one file in each application. The order of the steps is in
/// <see cref="Application.OpenDocument"/>; the spreadsheet adds its step through the hook.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/TemplateMethod.Demo -- gof-solution</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        Application draw = new DrawApplication();
        draw.OpenDocument("house.draw");
        draw.OpenDocument("budget.sheet");          // not a drawing: nothing happens
        Console.WriteLine("Draw:        " + Show(draw.Events));

        Application sheet = new SpreadsheetApplication();
        sheet.OpenDocument("budget.sheet");
        Console.WriteLine("Spreadsheet: " + Show(sheet.Events));
        Console.WriteLine("Documents the spreadsheet opened: " + sheet.Documents.Count);
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
