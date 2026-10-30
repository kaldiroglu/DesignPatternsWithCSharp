// Most examples have a class called Main or Test. Aliases name them apart. The namespace
// File would hide System.IO.File inside dev.kaldiroglu.Visitor, so the file examples are
// always reached through these aliases too.
using CheckoutMain = dev.kaldiroglu.Visitor.Checkout.Solution.Main;
using CheckoutMethodsMain = dev.kaldiroglu.Visitor.Checkout.Problem.Methods.Main;
using CheckoutStageTwoMain = dev.kaldiroglu.Visitor.Checkout.Problem.StageTwoMain;
using CheckoutStageThreeMain = dev.kaldiroglu.Visitor.Checkout.Problem.StageThreeMain;
using CheckoutGiftCardMain = dev.kaldiroglu.Visitor.Checkout.Problem.GiftCardMain;
using CheckoutModernMain = dev.kaldiroglu.Visitor.Checkout.Modern.Main;
using GofProblemMain = dev.kaldiroglu.Visitor.Gof.Problem.Main;
using GofSolutionMain = dev.kaldiroglu.Visitor.Gof.Solution.Main;
using AccountPrintMain = dev.kaldiroglu.Visitor.Hw.AccountPrint.Main;
using FileTreeMain = dev.kaldiroglu.Visitor.Hw.FileTree.Main;
using ExpressionMain = dev.kaldiroglu.Visitor.Hw.Expression.Main;
using FileProblem1Test = dev.kaldiroglu.Visitor.File.Problem1.Main;
using FileProblem2Test = dev.kaldiroglu.Visitor.File.Problem2.Main;
using FilePattern1Test = dev.kaldiroglu.Visitor.File.Pattern1.Main;
using FileAccessMain = dev.kaldiroglu.Visitor.File.Pattern1.AccessMain;
using FactoryTest = dev.kaldiroglu.Visitor.Factory.Main;
using InterpreterMain = dev.kaldiroglu.Visitor.Interpreter.Main;

namespace dev.kaldiroglu.Visitor.Demo;

/// <summary>
/// Runs the Visitor examples.
/// <para>
/// The file examples and <c>factory</c> choose at random, so their output changes from run to
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
        ["checkout-problem-stage-one"] = ("A CHECKOUT", CheckoutMethodsMain.Run),
        ["checkout-problem-stage-two"] = ("A CHECKOUT", CheckoutStageTwoMain.Run),
        ["checkout-problem-stage-three"] = ("A CHECKOUT", CheckoutStageThreeMain.Run),
        ["checkout-problem-gift-card"] = ("A CHECKOUT", CheckoutGiftCardMain.Run),
        ["checkout"] = ("A CHECKOUT", CheckoutMain.Run),
        ["checkout-modern"] = ("A CHECKOUT", CheckoutModernMain.Run),
        ["gof-problem"] = ("GOF'S COMPILER", GofProblemMain.Run),
        ["gof-solution"] = ("GOF'S COMPILER", GofSolutionMain.Run),
        ["file-problem1"] = ("TEXT FILES AND XML FILES", FileProblem1Test.Run),
        ["file-problem2"] = ("TEXT FILES AND XML FILES", FileProblem2Test.Run),
        ["file-pattern1"] = ("TEXT FILES AND XML FILES", FilePattern1Test.Run),
        ["file-access"] = ("TEXT FILES AND XML FILES", FileAccessMain.Run),
        ["factory"] = ("A HEALTH CHECK IN A COMPANY", FactoryTest.Run),
        ["interpreter"] = ("INTERPRETER: A RULE LANGUAGE", InterpreterMain.Run),
        ["hw-accountprint"] = ("HOMEWORK", AccountPrintMain.Run),
        ["hw-filetree"] = ("HOMEWORK", FileTreeMain.Run),
        ["hw-expression"] = ("HOMEWORK", ExpressionMain.Run)
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
