namespace dev.kaldiroglu.TemplateMethod.Hw.CallCenter;

/// <summary>
/// Imports the calls of three call centers. Ankara's second recording is too short, and
/// the verification step, which no call center can skip, rejects it.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/TemplateMethod.Demo -- hw-callcenter</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        List<CallImport> imports = [new IstanbulCallCenter(), new AnkaraCallCenter(), new IzmirCallCenter()];
        foreach (CallImport callImport in imports)
        {
            callImport.Run();
            Console.WriteLine(callImport.GetType().Name
                    + ": stored " + Show(callImport.Stored) + ", rejected " + Show(callImport.Rejected));
        }
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
