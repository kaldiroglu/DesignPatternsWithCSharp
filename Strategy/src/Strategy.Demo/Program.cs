// Several namespaces reuse a class name, as the Java original does: two Compositions, three
// Sorters and a Main in each example package. Aliases name them apart.
using dev.kaldiroglu.Strategy.Gof;
using dev.kaldiroglu.Strategy.Gof.Solution;
using dev.kaldiroglu.Strategy.Sorting.Pattern;
using NaiveComposition = dev.kaldiroglu.Strategy.Gof.Problem.Composition;
using Composition = dev.kaldiroglu.Strategy.Gof.Solution.Composition;
using NaiveSorter = dev.kaldiroglu.Strategy.Sorting.Problem.Sorter;
using GofLayoutMain = dev.kaldiroglu.Strategy.Gof.Main;
using GofProblemMain = dev.kaldiroglu.Strategy.Gof.Problem.Main;
using GofSolutionMain = dev.kaldiroglu.Strategy.Gof.Solution.Main;
using PricingProblemMain = dev.kaldiroglu.Strategy.Pricing.Problem.Main;
using PricingSolutionMain = dev.kaldiroglu.Strategy.Pricing.Solution.Main;
using SortingProblemMain = dev.kaldiroglu.Strategy.Sorting.Problem.Main;
using SortingSubclassingMain = dev.kaldiroglu.Strategy.Sorting.Subclassing.Main;
using SortingPatternMain = dev.kaldiroglu.Strategy.Sorting.Pattern.Main;
using FreightMain = dev.kaldiroglu.Strategy.Freight.Main;
using SeatingMain = dev.kaldiroglu.Strategy.Hw.Seating.Main;
using LateFeeMain = dev.kaldiroglu.Strategy.Hw.LateFee.Main;
using ValidationMain = dev.kaldiroglu.Strategy.Hw.Validation.Main;

namespace dev.kaldiroglu.Strategy.Demo;

/// <summary>
/// Runs each Strategy example once and prints the figures the course quotes about it.
/// <para>
/// Most entries call the port of a Java <c>main</c> method and print the same lines. Two are
/// only in this runner: <c>gof</c> sets GoF's compositors side by side with the naive
/// composition, and <c>sorting</c> sets the naive sorter beside the context in one table.
/// Each example runs on its own — <c>dotnet run -- freight</c> — and with no argument all of
/// them run in the order the course presents them.
/// </para>
/// </summary>
public static class Program
{
    private static readonly Dictionary<string, (string Group, Action Run)> Examples = new()
    {
        ["gof"] = ("GOF'S COMPOSITORS", Compositors),
        ["gof-layout"] = ("GOF'S COMPOSITORS", GofLayoutMain.Run),
        ["gof-problem"] = ("GOF'S COMPOSITORS", GofProblemMain.Run),
        ["gof-solution"] = ("GOF'S COMPOSITORS", GofSolutionMain.Run),
        ["pricing-problem"] = ("THE CHECKOUT", PricingProblemMain.Run),
        ["pricing-solution"] = ("THE CHECKOUT", PricingSolutionMain.Run),
        ["sorting"] = ("THE SORTER", Sorting),
        ["sorting-problem"] = ("THE SORTER", SortingProblemMain.Run),
        ["sorting-subclassing"] = ("THE SORTER", SortingSubclassingMain.Run),
        ["sorting-pattern"] = ("THE SORTER", SortingPatternMain.Run),
        ["freight"] = ("FREIGHT QUOTING", FreightMain.Run),
        ["hw-seating"] = ("HOMEWORK", SeatingMain.Run),
        ["hw-latefee"] = ("HOMEWORK", LateFeeMain.Run),
        ["hw-validation"] = ("HOMEWORK", ValidationMain.Run)
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

    // ------------------------------------------------------------ GoF's compositors

    private const string Paragraph =
        "A document editor breaks a stream of text into lines "
        + "and there are many algorithms for it";

    private const int Measure = 26;

    private static Composition DocumentWith(ICompositor compositor)
    {
        var document = new Composition(Measure, compositor);
        foreach (var word in Paragraph.Split(' '))
        {
            document.Insert(Component.Word(word));
        }
        return document;
    }

    private static void Compositors()
    {
        Console.WriteLine($"GoF's motivation sentence, in a {Measure}-column measure.");

        foreach (ICompositor compositor in new ICompositor[] { new SimpleCompositor(), new TeXCompositor() })
        {
            var layout = DocumentWith(compositor).Repair();
            Console.WriteLine($"\n{compositor.Name}: {layout.LineCount} lines, worst gap {layout.WorstSlack}");
            foreach (var line in layout.Render())
            {
                Console.WriteLine($"  |{line.PadRight(Measure)}|");
            }
        }

        var rows = DocumentWith(new ArrayCompositor(6)).Repair();
        Console.WriteLine($"\nArrayCompositor(6): {rows.Lines[0].Count} to the first row, "
                          + $"{rows.WidthOf(0)} wide in a {Measure}-column measure");

        var document = DocumentWith(new SimpleCompositor());
        document.SetCompositor(new TeXCompositor());
        Console.WriteLine($"\nThe same document, compositor replaced: now {document.CompositorName}");

        var words = Paragraph.Split(' ').Select(Component.Word).ToList();
        var fast = new NaiveComposition(words, Measure, false).Repair().Render();
        var quality = new NaiveComposition(words, Measure, true).Repair().Render();
        Console.WriteLine("The naive Composition lays the paragraph out the same way: "
                          + $"fast {Same(fast, DocumentWith(new SimpleCompositor()).Repair().Render())}, "
                          + $"quality {Same(quality, DocumentWith(new TeXCompositor()).Repair().Render())}");
    }

    private static string Same(IReadOnlyList<string> a, IReadOnlyList<string> b) =>
        a.SequenceEqual(b) ? "identical" : "DIFFERENT";

    // ------------------------------------------------------------------ the sorter

    private static double[] Shuffled(int size)
    {
        var random = new Random(42);          // fixed seed: the same array every run
        var list = new double[size];
        for (var i = 0; i < size; i++)
        {
            list[i] = random.NextDouble() * 1000;
        }
        return list;
    }

    private static void Sorting()
    {
        var naive = new NaiveSorter();
        var context = new SortingContext();

        Console.WriteLine("size        naive        context      sorted");
        foreach (var size in new[] { 10, 99, 100, 5_000 })
        {
            var a = Shuffled(size);
            var b = (double[])a.Clone();
            naive.Sort(a);
            context.Sort(b);
            var sorted = a.SequenceEqual(b) && a.Zip(a.Skip(1)).All(pair => pair.First <= pair.Second);
            Console.WriteLine($"{size,-11} {naive.LastUsed,-12} {context.LastUsed,-12} {sorted}");
        }

        Console.WriteLine($"\nThresholds: bubble below {SortingContext.BubbleLimit}, "
                          + $"the library from {SortingContext.QuickLimit}.");
        Console.WriteLine($"For a billion elements, without allocating them: "
                          + $"{context.SorterFor(1_000_000_000).Name}");
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
