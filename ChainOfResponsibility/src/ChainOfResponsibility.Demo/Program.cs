// Most examples have a class called Main or Test. Aliases name them apart.
using StageOneMain = dev.kaldiroglu.ChainOfResponsibility.Expense.Problem.StageOneMain;
using StageTwoMain = dev.kaldiroglu.ChainOfResponsibility.Expense.Problem.StageTwoMain;
using StageThreeMain = dev.kaldiroglu.ChainOfResponsibility.Expense.Problem.StageThreeMain;
using SixExpensesMain = dev.kaldiroglu.ChainOfResponsibility.Expense.Solution.SixExpensesMain;
using AuditLogMain = dev.kaldiroglu.ChainOfResponsibility.Expense.Solution.AuditLogMain;
using GofProblemMain = dev.kaldiroglu.ChainOfResponsibility.Gof.Problem.Main;
using GofSolutionMain = dev.kaldiroglu.ChainOfResponsibility.Gof.Solution.Main;
using MaintenanceMain = dev.kaldiroglu.ChainOfResponsibility.Hw.Maintenance.Main;
using MiddlewareMain = dev.kaldiroglu.ChainOfResponsibility.Hw.Middleware.Main;
using CashDispenserMain = dev.kaldiroglu.ChainOfResponsibility.Hw.CashDispenser.Main;
using CallCenterMain = dev.kaldiroglu.ChainOfResponsibility.CallCenter.Main;

namespace dev.kaldiroglu.ChainOfResponsibility.Demo;

/// <summary>
/// Runs the Chain of Responsibility examples.
/// <para>
/// <c>callcenter</c> chooses customers at random, so its output changes from run to
/// run.
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
        ["expense-problem-stage-one"] = ("WHO APPROVES AN EXPENSE", StageOneMain.Run),
        ["expense-problem-stage-two"] = ("WHO APPROVES AN EXPENSE", StageTwoMain.Run),
        ["expense-problem-stage-three"] = ("WHO APPROVES AN EXPENSE", StageThreeMain.Run),
        ["expense-six-expenses"] = ("WHO APPROVES AN EXPENSE", SixExpensesMain.Run),
        ["expense-audit-log"] = ("WHO APPROVES AN EXPENSE", AuditLogMain.Run),
        ["gof-problem"] = ("GOF'S CONTEXT-SENSITIVE HELP", GofProblemMain.Run),
        ["gof-solution"] = ("GOF'S CONTEXT-SENSITIVE HELP", GofSolutionMain.Run),
        ["hw-maintenance"] = ("HOMEWORK", MaintenanceMain.Run),
        ["hw-middleware"] = ("HOMEWORK", MiddlewareMain.Run),
        ["hw-cashdispenser"] = ("HOMEWORK", CashDispenserMain.Run),
        ["callcenter"] = ("A CALL CENTER", CallCenterMain.Run),
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
