using System.Globalization;
using dev.kaldiroglu.ChainOfResponsibility.Hw.CashDispenser;
using dev.kaldiroglu.ChainOfResponsibility.Hw.Maintenance;
using dev.kaldiroglu.ChainOfResponsibility.Hw.Middleware;
// Most examples have a class called Main or Test, as the Java original does. Aliases name
// them apart.
using ExpenseMain = dev.kaldiroglu.ChainOfResponsibility.Expense.Solution.Main;
using GofMain = dev.kaldiroglu.ChainOfResponsibility.Gof.Main;
using CallCenterTest = dev.kaldiroglu.ChainOfResponsibility.CallCenter.Test;
using PatternTest = dev.kaldiroglu.ChainOfResponsibility.Pattern.Test;

namespace dev.kaldiroglu.ChainOfResponsibility.Demo;

/// <summary>
/// Runs the Chain of Responsibility examples.
/// <para>
/// <c>expense</c>, <c>gof</c>, <c>callcenter</c> and <c>pattern</c> are the Java original's
/// <c>main</c> methods and print the same output. <c>callcenter</c> chooses customers at
/// random, so its output changes from run to run. The three homework exercises have no
/// <c>main</c> in Java and are only in this runner.
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
        ["expense"] = ("WHO APPROVES AN EXPENSE", ExpenseMain.Run),
        ["gof"] = ("GOF'S CONTEXT-SENSITIVE HELP", GofMain.Run),
        ["hw-maintenance"] = ("HOMEWORK", MaintenanceHomework),
        ["hw-middleware"] = ("HOMEWORK", MiddlewareHomework),
        ["hw-cashdispenser"] = ("HOMEWORK", CashDispenserHomework),
        ["callcenter"] = ("A CALL CENTER", CallCenterTest.Run),
        ["pattern"] = ("HELP TOPICS FROM SPECIFIC TO GENERAL", PatternTest.Run)
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

    /// <summary>
    /// Five requests go into the queue. The chain is a bug fixer, a UI designer, a feature
    /// developer and the project team. A 30-day improvement is too large for the feature
    /// developer and goes on to the project team.
    /// </summary>
    private static void MaintenanceHomework()
    {
        Developer first = new BugFixer("Ali");
        first.Then(new UiDesigner("Zeynep")).Then(new FeatureDeveloper("Kerem")).Then(new ProjectTeam("the project team"));

        RequestQueue queue = new RequestQueue(first);
        queue.Add(new Request(RequestKind.BUG, "Login fails", 1));
        queue.Add(new Request(RequestKind.IMPROVEMENT, "Faster search", 4));
        queue.Add(new Request(RequestKind.UI_CHANGE, "New logo", 2));
        queue.Add(new Request(RequestKind.IMPROVEMENT, "New report engine", 30));
        queue.Add(new Request(RequestKind.PROJECT, "Mobile app", 120));
        foreach (string assignment in queue.ProcessAll())
        {
            Console.WriteLine(assignment);
        }
    }

    /// <summary>
    /// Logging, an admin check and a header around one endpoint. The admin check answers 403
    /// itself, and the log shows that every request went through the logging link first.
    /// </summary>
    private static void MiddlewareHomework()
    {
        List<string> log = [];
        Endpoint app = MiddlewareChain.Chain(
                [Links.Logging(log), Links.AdminOnly(), Links.PoweredBy()],
                request => "200 " + request.Path);

        Console.WriteLine(app(new HttpRequest("/home", "elif")));
        Console.WriteLine(app(new HttpRequest("/admin/users", "elif")));
        Console.WriteLine(app(new HttpRequest("/admin/users", "admin")));
        Console.WriteLine("log " + Show(log));
    }

    /// <summary>
    /// Slots of 200, 100, 50 and 20. 370 and 380 are paid; 260 leaves 10 that cannot be paid,
    /// although 100 + 100 + 20 + 20 + 20 is 260.
    /// </summary>
    private static void CashDispenserHomework()
    {
        NoteSlot atm = new NoteSlot(200);
        atm.Then(new NoteSlot(100)).Then(new NoteSlot(50)).Then(new NoteSlot(20));
        foreach (int amount in new[] { 370, 380, 260 })
        {
            Console.WriteLine(amount.ToString(CultureInfo.InvariantCulture) + ": " + Show(atm.Pay(amount)));
        }
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";

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
