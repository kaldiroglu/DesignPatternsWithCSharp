using dev.kaldiroglu.Mediator.Gof.Problem;
using dev.kaldiroglu.Mediator.Gof.Solution;

namespace dev.kaldiroglu.Mediator.Gof;

/// <summary>The same steps in both designs: select a font, clear the field, type one, click OK.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Mediator.Demo -- gof</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        FontDialog before = new FontDialog();
        before.FontList.Select("Helvetica");
        Console.WriteLine("Before the pattern");
        Console.WriteLine("  after selecting: field '" + before.FontName.Text + "', OK enabled " + Show(before.Ok.Enabled));
        before.FontName.SetText("");
        Console.WriteLine("  after clearing:  OK enabled " + Show(before.Ok.Enabled));
        before.FontName.SetText("Times");
        before.Ok.Click(before.FontName.Text);
        Console.WriteLine("  after OK:        " + Show(before.Log));

        FontDialogDirector after = new FontDialogDirector();
        after.FontList.Select("Helvetica");
        Console.WriteLine("With a director");
        Console.WriteLine("  after selecting: field '" + after.FontName.Text + "', OK enabled " + Show(after.Ok.Enabled));
        after.FontName.Type("");
        Console.WriteLine("  after clearing:  OK enabled " + Show(after.Ok.Enabled));
        after.FontName.Type("Times");
        after.Ok.Click();
        Console.WriteLine("  after OK:        " + Show(after.Log));
    }

    /// <summary>Prints a boolean the way Java does: <c>true</c> or <c>false</c>, not <c>True</c>.</summary>
    private static string Show(bool value) => value ? "true" : "false";

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
