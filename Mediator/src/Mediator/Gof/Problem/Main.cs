namespace dev.kaldiroglu.Mediator.Gof.Problem;

/// <summary>
/// Selects a font, clears the field, types a font and clicks OK. The widgets call each
/// other directly: the list box sets the field, the field enables the button.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Mediator.Demo -- gof-problem</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        FontDialog dialog = new FontDialog();
        dialog.FontList.Select("Helvetica");
        Console.WriteLine("After selecting: field '" + dialog.FontName.Text + "', OK enabled " + Show(dialog.Ok.Enabled));
        dialog.FontName.SetText("");
        Console.WriteLine("After clearing:  OK enabled " + Show(dialog.Ok.Enabled));
        dialog.FontName.SetText("Times");
        dialog.Ok.Click(dialog.FontName.Text);
        Console.WriteLine("After OK:        " + Show(dialog.Log));
        Console.WriteLine("ListBox holds the EntryField, and EntryField holds the Button.");
    }

    /// <summary>Prints a boolean the way Java does: <c>true</c> or <c>false</c>, not <c>True</c>.</summary>
    private static string Show(bool value) => value ? "true" : "false";

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
