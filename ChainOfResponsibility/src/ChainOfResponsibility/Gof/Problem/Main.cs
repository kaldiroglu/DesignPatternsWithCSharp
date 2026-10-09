namespace dev.kaldiroglu.ChainOfResponsibility.Gof.Problem;

/// <summary>
/// Asks the help desk for help on four controls. One switch knows every control by name, and
/// a control it does not know gets the editor's general help.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/ChainOfResponsibility.Demo -- gof-problem</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        HelpDesk desk = new HelpDesk();
        foreach (string control in new[] { "print button", "ok button", "printer list", "font button" })
        {
            Console.WriteLine(control + ": " + desk.HelpFor(control));
        }
        Console.WriteLine("A new button is one more case in HelpDesk.helpFor.");
    }
}
