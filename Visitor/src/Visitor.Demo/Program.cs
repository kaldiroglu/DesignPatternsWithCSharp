// Most examples have a class called Main or Test, as the Java original does. Aliases name
// them apart. The namespace File would hide System.IO.File inside dev.kaldiroglu.Visitor, so
// the file examples are always reached through these aliases too.
using CheckoutMain = dev.kaldiroglu.Visitor.Checkout.Solution.Main;
using CheckoutMethodsMain = dev.kaldiroglu.Visitor.Checkout.Problem.Methods.Main;
using CheckoutProblemMain = dev.kaldiroglu.Visitor.Checkout.Problem.Main;
using CheckoutModernMain = dev.kaldiroglu.Visitor.Checkout.Modern.Main;
using GofMain = dev.kaldiroglu.Visitor.Gof.Main;
using GofProblemMain = dev.kaldiroglu.Visitor.Gof.Problem.Main;
using GofSolutionMain = dev.kaldiroglu.Visitor.Gof.Solution.Main;
using PatternProblemMain = dev.kaldiroglu.Visitor.Pattern.Problem.Main;
using AccountPrintMain = dev.kaldiroglu.Visitor.Hw.AccountPrint.Main;
using FileTreeMain = dev.kaldiroglu.Visitor.Hw.FileTree.Main;
using ExpressionMain = dev.kaldiroglu.Visitor.Hw.Expression.Main;
using FileProblem1Test = dev.kaldiroglu.Visitor.File.Problem1.Test;
using FileProblem2Test = dev.kaldiroglu.Visitor.File.Problem2.Test;
using FilePattern1Test = dev.kaldiroglu.Visitor.File.Pattern1.Test;
using FactoryTest = dev.kaldiroglu.Visitor.Factory.Test;
using InterpreterMain = dev.kaldiroglu.Visitor.Interpreter.Main;
using AnimalProblemTest = dev.kaldiroglu.Visitor.Animal.Problem.Test;
using AnimalPattern1Test = dev.kaldiroglu.Visitor.Animal.Pattern1.Test;
using AnimalPattern2Test = dev.kaldiroglu.Visitor.Animal.Pattern2.Test;

namespace dev.kaldiroglu.Visitor.Demo;

/// <summary>
/// Runs the Visitor examples.
/// <para>
/// Every entry is one of the Java original's <c>main</c> methods and prints the same output.
/// The file examples and <c>factory</c> choose at random, so their output changes from run to
/// run. The <c>Pattern</c> outline has empty methods, so <c>pattern-problem</c> only names its
/// classes and methods.
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
        ["checkout-methods"] = ("A CHECKOUT", CheckoutMethodsMain.Run),
        ["checkout-problem"] = ("A CHECKOUT", CheckoutProblemMain.Run),
        ["checkout"] = ("A CHECKOUT", CheckoutMain.Run),
        ["checkout-modern"] = ("A CHECKOUT", CheckoutModernMain.Run),
        ["gof-problem"] = ("GOF'S COMPILER", GofProblemMain.Run),
        ["gof-solution"] = ("GOF'S COMPILER", GofSolutionMain.Run),
        ["gof"] = ("GOF'S COMPILER", GofMain.Run),
        ["pattern-problem"] = ("GOF'S COMPILER, SHORT FORM", PatternProblemMain.Run),
        ["file-problem1"] = ("TEXT FILES AND XML FILES", FileProblem1Test.Run),
        ["file-problem2"] = ("TEXT FILES AND XML FILES", FileProblem2Test.Run),
        ["file-pattern1"] = ("TEXT FILES AND XML FILES", FilePattern1Test.Run),
        ["factory"] = ("A HEALTH CHECK IN A COMPANY", FactoryTest.Run),
        ["interpreter"] = ("INTERPRETER: A RULE LANGUAGE", InterpreterMain.Run),
        ["animal-problem"] = ("ANIMALS AND FEEDERS", AnimalProblemTest.Run),
        ["animal-pattern1"] = ("ANIMALS AND FEEDERS", AnimalPattern1Test.Run),
        ["animal-pattern2"] = ("ANIMALS AND FEEDERS", AnimalPattern2Test.Run),
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
