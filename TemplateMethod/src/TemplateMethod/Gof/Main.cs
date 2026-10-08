using dev.kaldiroglu.TemplateMethod.Gof.Solution;

namespace dev.kaldiroglu.TemplateMethod.Gof;

/// <summary>Opens one file of each kind, and one that the application cannot open.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/TemplateMethod.Demo -- gof</c>.
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
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
