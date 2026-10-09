namespace dev.kaldiroglu.TemplateMethod.Hw.Onboarding;

/// <summary>
/// The first day of an employee and of a contractor. The contractor overrides the
/// equipment hook, so the contractor gets no laptop.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/TemplateMethod.Demo -- hw-onboarding</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        Console.WriteLine("Employee Elif:   " + Show(new EmployeeOnboarding().Start("Elif")));
        Console.WriteLine("Contractor Mert: " + Show(new ContractorOnboarding().Start("Mert")));
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
