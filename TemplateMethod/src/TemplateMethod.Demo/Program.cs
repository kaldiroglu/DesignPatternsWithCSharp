// Several examples have a class called Main or Test, as the Java original does.
// Aliases name them apart.
using ExportMain = dev.kaldiroglu.TemplateMethod.Export.Solution.Main;
using ExportProblemMain = dev.kaldiroglu.TemplateMethod.Export.Problem.Main;
using GofMain = dev.kaldiroglu.TemplateMethod.Gof.Main;
using GofProblemMain = dev.kaldiroglu.TemplateMethod.Gof.Problem.Main;
using GofSolutionMain = dev.kaldiroglu.TemplateMethod.Gof.Solution.Main;
using CallCenterMain = dev.kaldiroglu.TemplateMethod.Hw.CallCenter.Main;
using OnboardingMain = dev.kaldiroglu.TemplateMethod.Hw.Onboarding.Main;
using RecordFileMain = dev.kaldiroglu.TemplateMethod.Hw.RecordFile.Main;
using PatternTest = dev.kaldiroglu.TemplateMethod.Pattern.Test;
using TaskTest = dev.kaldiroglu.TemplateMethod.Task.Test;

namespace dev.kaldiroglu.TemplateMethod.Demo;

/// <summary>
/// Runs the Template Method examples.
/// <para>
/// Every entry is one of the Java original's <c>main</c> methods and prints the same output.
/// <c>task</c> runs with an interval of 0 seconds instead of 1, so it prints the same lines
/// without waiting ten seconds.
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
        ["export-problem"] = ("THE REPORT EXPORT", ExportProblemMain.Run),
        ["export"] = ("THE REPORT EXPORT", ExportMain.Run),
        ["gof-problem"] = ("GOF'S APPLICATIONS AND DOCUMENTS", GofProblemMain.Run),
        ["gof-solution"] = ("GOF'S APPLICATIONS AND DOCUMENTS", GofSolutionMain.Run),
        ["gof"] = ("GOF'S APPLICATIONS AND DOCUMENTS", GofMain.Run),
        ["pattern"] = ("APPLICATION AND DOCUMENT, SHORT FORM", PatternTest.Run),
        ["task"] = ("A REPEATED TASK", () => TaskTest.Run(interval: 0)),
        ["hw-callcenter"] = ("HOMEWORK", CallCenterMain.Run),
        ["hw-onboarding"] = ("HOMEWORK", OnboardingMain.Run),
        ["hw-recordfile"] = ("HOMEWORK", RecordFileMain.Run)
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

    // ---------------------------------------------------------------- output

    private static void Heading(string title)
    {
        Console.WriteLine("\n" + new string('=', 72));
        Console.WriteLine(title);
        Console.WriteLine(new string('=', 72));
    }

    private static void Section(string title) =>
        Console.WriteLine($"\n--- {title} {new string('-', Math.Max(0, 68 - title.Length))}");
}
