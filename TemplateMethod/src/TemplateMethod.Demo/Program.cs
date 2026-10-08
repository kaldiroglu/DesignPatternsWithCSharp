using dev.kaldiroglu.TemplateMethod.Hw.CallCenter;
using dev.kaldiroglu.TemplateMethod.Hw.Onboarding;
using dev.kaldiroglu.TemplateMethod.Hw.RecordFile;
// Several examples have a class called Main or Test, as the Java original does.
// Aliases name them apart.
using ExportMain = dev.kaldiroglu.TemplateMethod.Export.Solution.Main;
using GofMain = dev.kaldiroglu.TemplateMethod.Gof.Main;
using PatternTest = dev.kaldiroglu.TemplateMethod.Pattern.Test;
using TaskTest = dev.kaldiroglu.TemplateMethod.Task.Test;

namespace dev.kaldiroglu.TemplateMethod.Demo;

/// <summary>
/// Runs the Template Method examples.
/// <para>
/// <c>export</c>, <c>gof</c>, <c>pattern</c> and <c>task</c> are the Java original's
/// <c>main</c> methods and print the same output. <c>task</c> runs with an interval of 0
/// seconds instead of 1, so it prints the same lines without waiting ten seconds. The three
/// homework exercises have no <c>main</c> in Java and are only in this runner.
/// </para>
/// <para>
/// Each example runs on its own — <c>dotnet run -- gof</c> — and with no argument all of them
/// run in the order the course presents them.
/// </para>
/// </summary>
public static class Program
{
    private static readonly Dictionary<string, (string Group, Action Run)> Examples = new()
    {
        ["export"] = ("THE REPORT EXPORT", ExportMain.Run),
        ["gof"] = ("GOF'S APPLICATIONS AND DOCUMENTS", GofMain.Run),
        ["pattern"] = ("APPLICATION AND DOCUMENT, SHORT FORM", PatternTest.Run),
        ["task"] = ("A REPEATED TASK", () => TaskTest.Run(interval: 0)),
        ["hw-callcenter"] = ("HOMEWORK", CallCenter),
        ["hw-onboarding"] = ("HOMEWORK", Onboarding),
        ["hw-recordfile"] = ("HOMEWORK", RecordFile)
    };

    public static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            var name = args[0].ToLowerInvariant();
            if (!Examples.TryGetValue(name, out var example))
            {
                Console.WriteLine($"unknown example '{name}'. One of: {string.Join(", ", Examples.Keys)}");
                return;
            }

            example.Run();
            return;
        }

        string? lastGroup = null;
        foreach (var (name, (group, run)) in Examples)
        {
            if (group != lastGroup)
            {
                Heading(group);
                lastGroup = group;
            }

            Section(name);
            run();
        }
    }

    // ------------------------------------------------------------ homework

    /// <summary>Imports the three call centers. Ankara's audio for ANK-2 is cut short.</summary>
    private static void CallCenter()
    {
        foreach (var (city, import) in new (string, CallImport)[]
                 {
                     ("Istanbul", new IstanbulCallCenter()),
                     ("Ankara", new AnkaraCallCenter()),
                     ("Izmir", new IzmirCallCenter())
                 })
        {
            import.Run();
            Console.WriteLine($"{city + ":",-10}stored {Show(import.Stored)}, rejected {Show(import.Rejected)}");
        }
    }

    /// <summary>The first day of an employee and of a contractor.</summary>
    private static void Onboarding()
    {
        Console.WriteLine("Employee:   " + Show(new EmployeeOnboarding().Start("Ayse")));
        Console.WriteLine("Contractor: " + Show(new ContractorOnboarding().Start("Deniz")));
    }

    /// <summary>Reads two customers past a blank line, then a file with a bad line.</summary>
    private static void RecordFile()
    {
        var good = new ClosingReader("Ayse;Istanbul\n\nDeniz;Ankara\n");
        var customers = new CustomerFileReader().ReadAll(good);
        Console.WriteLine("Read: " + Show(customers.Select(c => c.Name + " (" + c.City + ")"))
            + ", reader closed: " + (good.Closed ? "true" : "false"));

        var bad = new ClosingReader("Ayse;Istanbul\nDeniz Ankara\n");
        try
        {
            new CustomerFileReader().ReadAll(bad);
            Console.WriteLine("Bad line: no error");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine("Bad line: " + e.Message + ", reader closed: " + (bad.Closed ? "true" : "false"));
        }
    }

    /// <summary>A reader that remembers whether it was closed, to show that the template method closes it.</summary>
    private sealed class ClosingReader(string text) : StringReader(text)
    {
        public bool Closed { get; private set; }

        protected override void Dispose(bool disposing)
        {
            Closed = true;
            base.Dispose(disposing);
        }
    }

    // ---------------------------------------------------------------- output

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";

    private static void Heading(string title)
    {
        Console.WriteLine("\n" + new string('=', 72));
        Console.WriteLine(title);
        Console.WriteLine(new string('=', 72));
    }

    private static void Section(string title) =>
        Console.WriteLine($"\n--- {title} {new string('-', Math.Max(0, 68 - title.Length))}");
}
