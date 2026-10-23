// Most example packages have a class named Main. Aliases name
// them apart; a plain `using` would make `Main` ambiguous. The account and the GoF solution
// have one class per slide, reached through their namespace aliases.
using AccountProblem = dev.kaldiroglu.Command.Account.Problem;
using AccountSolution = dev.kaldiroglu.Command.Account.Solution;
using AccountLambda = dev.kaldiroglu.Command.Account.Lambda;
using GofMain = dev.kaldiroglu.Command.Gof.Main;
using GofProblem = dev.kaldiroglu.Command.Gof.Problem.Main;
using GofSolution = dev.kaldiroglu.Command.Gof.Solution;
using LenderProblem1 = dev.kaldiroglu.Command.Lender.Problem1.Main;
using LenderProblem2 = dev.kaldiroglu.Command.Lender.Problem2.Main;
using LenderPattern = dev.kaldiroglu.Command.Lender.Pattern.Main;
using LenderLambda = dev.kaldiroglu.Command.Lender.Lambda.Main;
using AcLambda = dev.kaldiroglu.Command.Ac.Lambda.Main;
using Person = dev.kaldiroglu.Command.Ac.Person;
using HwKitchen = dev.kaldiroglu.Command.Hw.Kitchen.Main;
using HwMacro = dev.kaldiroglu.Command.Hw.Macro.Main;
using HwRemote = dev.kaldiroglu.Command.Hw.Remote.Main;

namespace dev.kaldiroglu.Command.Demo;

/// <summary>
/// Runs the examples: the teller's
/// account, GoF's menus, the lender's two problems and its pattern form, the air
/// conditioner, and the three homework solutions.
/// <para>
/// Each example runs on its own — <c>dotnet run -- lender-pattern</c> — so none of them
/// needs a line commented out to be seen alone. With no argument, all of them run in the
/// order the course presents them.
/// </para>
/// </summary>
public static class Program
{
    private static readonly Dictionary<string, (string Group, Action Run)> Examples = new()
    {
        ["account-problem-stage-one"] = ("THE TELLER", AccountProblem.StageOneMain.Run),
        ["account-problem-stage-two"] = ("THE TELLER", AccountProblem.StageTwoMain.Run),
        ["account-problem-transfer-undo"] = ("THE TELLER", AccountProblem.TransferUndoMain.Run),
        ["account-transfer"] = ("THE TELLER", AccountSolution.TransferMain.Run),
        ["account-close-out"] = ("THE TELLER", AccountSolution.CloseOutMain.Run),
        ["account-standing-orders"] = ("THE TELLER", AccountSolution.StandingOrdersMain.Run),
        ["account-lambda-transfer"] = ("THE TELLER", AccountLambda.TransferMain.Run),
        ["account-lambda-close-out"] = ("THE TELLER", AccountLambda.CloseOutMain.Run),
        ["account-lambda-standing-orders"] = ("THE TELLER", AccountLambda.StandingOrdersMain.Run),
        ["gof"] = ("GOF'S MENUS", GofMain.Run),
        ["gof-problem"] = ("GOF'S MENUS", GofProblem.Run),
        ["gof-paste-command"] = ("GOF'S MENUS", GofSolution.PasteCommandMain.Run),
        ["gof-three-commands"] = ("GOF'S MENUS", GofSolution.ThreeCommandsMain.Run),
        ["gof-simple-command"] = ("GOF'S MENUS", GofSolution.SimpleCommandMain.Run),
        ["lender-problem1"] = ("THE LENDER", LenderProblem1.Run),
        ["lender-problem2"] = ("THE LENDER", LenderProblem2.Run),
        ["lender-pattern"] = ("THE LENDER", LenderPattern.Run),
        ["lender-lambda"] = ("THE LENDER", LenderLambda.Run),
        ["ac"] = ("THE AIR CONDITIONER", Person.Run),
        ["ac-lambda"] = ("THE AIR CONDITIONER", AcLambda.Run),
        ["hw-remote"] = ("HOMEWORK", HwRemote.Run),
        ["hw-kitchen"] = ("HOMEWORK", HwKitchen.Run),
        ["hw-macro"] = ("HOMEWORK", HwMacro.Run)
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
