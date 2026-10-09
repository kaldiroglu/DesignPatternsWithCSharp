namespace dev.kaldiroglu.TemplateMethod.Gof.Problem;

/// <summary>
/// Opens one file in each application. Each application has its own copy of the steps,
/// and the spreadsheet added its extra step in a place it chose.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/TemplateMethod.Demo -- gof-problem</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var draw = new DrawApplication();
        draw.OpenDocument("house.draw");
        draw.OpenDocument("budget.sheet");          // not a drawing: nothing happens
        Console.WriteLine("Draw:        " + Show(draw.Events));

        var sheet = new SpreadsheetApplication();
        sheet.OpenDocument("budget.sheet");
        Console.WriteLine("Spreadsheet: " + Show(sheet.Events));
        Console.WriteLine("The same steps are written twice, once in each class.");
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
