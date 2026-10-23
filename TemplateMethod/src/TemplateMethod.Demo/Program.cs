// Several examples have a class called Main or Test. Aliases name them apart.
using ExportStageOneMain = dev.kaldiroglu.TemplateMethod.Export.Problem.StageOneMain;
using ExportStageTwoMain = dev.kaldiroglu.TemplateMethod.Export.Problem.StageTwoMain;
using ExportMissingAuditMain = dev.kaldiroglu.TemplateMethod.Export.Problem.MissingAuditMain;
using ExportThreeExportsMain = dev.kaldiroglu.TemplateMethod.Export.Solution.ThreeExportsMain;
using ExportAuditLogsMain = dev.kaldiroglu.TemplateMethod.Export.Solution.AuditLogsMain;
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
/// Each entry runs one example and prints what one slide of the deck shows.
/// <c>task</c> runs with an interval of 0 seconds instead of 1, so it prints the same lines
/// without waiting ten seconds.
/// </para>
/// <para>
/// Each example runs on its own — <c>dotnet run -- gof-solution</c> — and with no argument all of them
/// run in the order the course presents them.
/// </para>
/// </summary>
public static class Program
{
    private static readonly Dictionary<string, (string Group, Action Run)> Examples = new()
    {
        ["export-problem-stage-one"] = ("THE REPORT EXPORT", ExportStageOneMain.Run),
        ["export-problem-stage-two"] = ("THE REPORT EXPORT", ExportStageTwoMain.Run),
        ["export-problem-missing-audit"] = ("THE REPORT EXPORT", ExportMissingAuditMain.Run),
        ["export-three-exports"] = ("THE REPORT EXPORT", ExportThreeExportsMain.Run),
        ["export-audit-logs"] = ("THE REPORT EXPORT", ExportAuditLogsMain.Run),
        ["gof-problem"] = ("GOF'S APPLICATIONS AND DOCUMENTS", GofProblemMain.Run),
        ["gof-solution"] = ("GOF'S APPLICATIONS AND DOCUMENTS", GofSolutionMain.Run),
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
