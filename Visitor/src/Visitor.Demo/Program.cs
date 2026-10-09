using dev.kaldiroglu.Visitor.Hw.AccountPrint;
using dev.kaldiroglu.Visitor.Hw.Expression;
using dev.kaldiroglu.Visitor.Hw.FileTree;
// Most examples have a class called Main or Test, as the Java original does. Aliases name
// them apart. The namespace File would hide System.IO.File inside dev.kaldiroglu.Visitor, so
// the file examples are always reached through these aliases too.
using CheckoutMain = dev.kaldiroglu.Visitor.Checkout.Solution.Main;
using GofMain = dev.kaldiroglu.Visitor.Gof.Main;
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
/// <c>checkout</c>, <c>gof</c>, <c>file-problem1</c>, <c>file-problem2</c>,
/// <c>file-pattern1</c>, <c>factory</c>, <c>interpreter</c>, <c>animal-problem</c>, <c>animal-pattern1</c> and
/// <c>animal-pattern2</c> are the Java original's <c>main</c> methods and print the same
/// output. The file examples and <c>factory</c> choose at random, so their output changes
/// from run to run. The three homework exercises have no <c>main</c> in Java and are only in
/// this runner. The <c>Pattern</c> outline has empty methods, so there is nothing to run.
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
        ["checkout"] = ("A CHECKOUT", CheckoutMain.Run),
        ["gof"] = ("GOF'S COMPILER", GofMain.Run),
        ["file-problem1"] = ("TEXT FILES AND XML FILES", FileProblem1Test.Run),
        ["file-problem2"] = ("TEXT FILES AND XML FILES", FileProblem2Test.Run),
        ["file-pattern1"] = ("TEXT FILES AND XML FILES", FilePattern1Test.Run),
        ["factory"] = ("A HEALTH CHECK IN A COMPANY", FactoryTest.Run),
        ["interpreter"] = ("INTERPRETER: A RULE LANGUAGE", InterpreterMain.Run),
        ["animal-problem"] = ("ANIMALS AND FEEDERS", AnimalProblemTest.Run),
        ["animal-pattern1"] = ("ANIMALS AND FEEDERS", AnimalPattern1Test.Run),
        ["animal-pattern2"] = ("ANIMALS AND FEEDERS", AnimalPattern2Test.Run),
        ["hw-accountprint"] = ("HOMEWORK", AccountPrintHomework),
        ["hw-filetree"] = ("HOMEWORK", FileTreeHomework),
        ["hw-expression"] = ("HOMEWORK", ExpressionHomework)
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
    /// Three accounts printed twice: as plain lines to the console, and as HTML rows into a
    /// string, which is printed afterwards. The accounts are the same objects both times.
    /// </summary>
    private static void AccountPrintHomework()
    {
        List<IAccount> bank =
        [
            new CheckingAccount("Deniz", 1200, 500),
            new SavingsAccount("Elif", 8000, 3),
            new LoanAccount("Mert", 20000, 900)
        ];
        var text = new TextPrinter(Console.Out);
        bank.ForEach(account => account.Accept(text));

        var html = new StringWriter();
        var htmlPrinter = new HtmlPrinter(html);
        bank.ForEach(account => account.Accept(htmlPrinter));
        Console.Write(html);
    }

    /// <summary>
    /// A project folder with a README, a <c>src</c> folder of two files and a <c>docs</c>
    /// folder of one. The folders walk themselves; the visitors only add up and list.
    /// </summary>
    private static void FileTreeHomework()
    {
        var root = new Folder("project",
                new FileEntry("README.md", 120),
                new Folder("src", new FileEntry("Main.java", 900), new FileEntry("Util.java", 300)),
                new Folder("docs", new FileEntry("guide.pdf", 4000)));

        var size = new SizeVisitor();
        root.Accept(size);
        Console.WriteLine("size " + size.Total);

        var listing = new ListingVisitor();
        root.Accept(listing);
        foreach (var line in listing.Lines)
        {
            Console.WriteLine(line);
        }
    }

    /// <summary>(2 + 3) * -4, printed, evaluated and measured by three visitors.</summary>
    private static void ExpressionHomework()
    {
        IExpr e = new Mul(new Add(new Num(2), new Num(3)), new Neg(new Num(4)));
        Console.WriteLine(e.Accept(new Printer()) + " = "
                + e.Accept(new Evaluator()).ToString(System.Globalization.CultureInfo.InvariantCulture)
                + ", depth " + e.Accept(new DepthCounter()));
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
