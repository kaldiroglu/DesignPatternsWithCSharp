namespace dev.kaldiroglu.Mediator.Gof.Solution;

/// <summary>
/// Selects a font, clears the field, types a font, clicks OK, then Cancel. Every widget
/// reports to the director, and the director decides what changes.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Mediator.Demo -- gof-solution</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        FontDialogDirector director = new FontDialogDirector();
        director.FontList.Select("Helvetica");
        Console.WriteLine("After selecting: field '" + director.FontName.Text + "', OK enabled " + Show(director.Ok.Enabled));
        director.FontName.Type("");
        Console.WriteLine("After clearing:  OK enabled " + Show(director.Ok.Enabled));
        director.FontName.Type("Times");
        director.Ok.Click();
        director.Cancel.Click();
        Console.WriteLine("After OK and Cancel: " + Show(director.Log));
    }

    /// <summary>Prints a boolean the way Java does: <c>true</c> or <c>false</c>, not <c>True</c>.</summary>
    private static string Show(bool value) => value ? "true" : "false";

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
