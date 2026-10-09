using dev.kaldiroglu.Strategy.Sorting.Pattern;
using Xunit;
using NaiveSorter = dev.kaldiroglu.Strategy.Sorting.Problem.Sorter;
using SubclassBubbleSorter = dev.kaldiroglu.Strategy.Sorting.Subclassing.BubbleSorter;

namespace dev.kaldiroglu.Strategy.Tests.Sorting;

/// <summary>
/// Three sorting algorithms, and the array's size decides which one is used. The branch does
/// not disappear when the pattern is applied: it stops implementing and starts selecting.
/// Ported from the Java <c>sorting.SortingTest</c>.
/// </summary>
public class SortingTests
{
    private const string Source = "Sorting/";

    private static double[] Shuffled(int size)
    {
        // A fixed seed: the same array every run. The numbers differ from Java's Random(42),
        // but every test here compares C# results with other C# results.
        var random = new Random(42);
        var list = new double[size];
        for (var i = 0; i < size; i++)
        {
            list[i] = random.NextDouble() * 1000;
        }
        return list;
    }

    private static double[] SortedCopy(double[] list)
    {
        var copy = (double[])list.Clone();
        Array.Sort(copy);
        return copy;
    }

    [Fact(DisplayName = "all three designs sort the same array into the same order")]
    public void TheDesignsAgree()
    {
        var expected = SortedCopy(Shuffled(50));

        var byBranch = Shuffled(50);
        new NaiveSorter().Sort(byBranch);

        var bySubclass = Shuffled(50);
        new SubclassBubbleSorter().Sort(bySubclass);

        var byStrategy = Shuffled(50);
        new SortingContext().Sort(byStrategy);

        Assert.Equal(expected, byBranch);
        Assert.Equal(expected, bySubclass);
        Assert.Equal(expected, byStrategy);
    }

    [Fact(DisplayName = "the size decides, and both designs decide the same way")]
    public void TheSameThresholds()
    {
        var naive = new NaiveSorter();
        var context = new SortingContext();

        foreach (var size in new[] { 10, 99, 100, 5_000 })
        {
            var a = Shuffled(size);
            var b = (double[])a.Clone();
            naive.Sort(a);
            context.Sort(b);
            Assert.Equal(naive.LastUsed, context.LastUsed);
        }

        Assert.Equal("BubbleSort", context.SorterFor(99).Name);
        Assert.Equal("QuickSort", context.SorterFor(100).Name);
        Assert.Equal("JavaSort", context.SorterFor(1_000_000).Name);
    }

    [Fact(DisplayName = "the decision can be tested without sorting anything")]
    public void TheDecisionIsSeparable()
    {
        var context = new SortingContext();

        // Which algorithm for a billion elements, without allocating a billion doubles.
        Assert.Equal("JavaSort", context.SorterFor(1_000_000_000).Name);
        Assert.Same(context.SorterFor(50), context.SorterFor(60));
        Assert.NotSame(context.SorterFor(50), context.SorterFor(5_000));
    }

    [Fact(DisplayName = "the branch survives the pattern — but it selects instead of implementing")]
    public void TheBranchMovedRatherThanVanished()
    {
        var naive = SourceText.Read(Source + "Problem/Sorter.cs");
        var context = SourceText.Read(Source + "Pattern/SortingContext.cs");
        var code = SourceText.StripComments(context);

        // Both test the same two thresholds. Java names the constants BUBBLE_LIMIT and
        // QUICK_LIMIT; the C# constants are BubbleLimit and QuickLimit.
        Assert.True(naive.Contains("100") && naive.Contains("1_000_000"));
        Assert.True(code.Contains("BubbleLimit") && code.Contains("QuickLimit"));

        // The naive class carries the algorithms too: a bubble pass, a quicksort and a
        // partition. The context carries none of them.
        Assert.True(naive.Contains("Quicksort(") && naive.Contains("Partition("));
        Assert.True(code.Contains("return _bubbleSorter") && !code.Contains("Partition("),
            "the context selects; it must not implement");

        // The naive class is longer than the one that only decides.
        var naiveLines = naive.Split('\n').Length;
        var contextLines = context.Split('\n').Length;
        Assert.True(naiveLines > contextLines, naiveLines + " lines against " + contextLines);
    }

    /// <summary>
    /// A fourth algorithm. Java writes it as an anonymous class inside the test; C# has no
    /// anonymous classes that implement an interface, so it is a nested class here.
    /// </summary>
    private sealed class InsertionSorter : ISorter
    {
        public string Name => "InsertionSort";

        public void Sort(double[] list)
        {
            for (var i = 1; i < list.Length; i++)
            {
                var key = list[i];
                var j = i - 1;
                while (j >= 0 && list[j] > key)
                {
                    list[j + 1] = list[j];
                    j--;
                }
                list[j + 1] = key;
            }
        }
    }

    [Fact(DisplayName = "a fourth algorithm is one class, and the context is the only edit")]
    public void AddingAnAlgorithm()
    {
        ISorter insertion = new InsertionSorter();

        var list = Shuffled(30);
        var expected = SortedCopy(list);
        insertion.Sort(list);

        Assert.Equal(expected, list);
        Assert.Equal("InsertionSort", insertion.Name);
    }
}
