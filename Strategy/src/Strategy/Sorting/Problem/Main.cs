using System.Globalization;

namespace dev.kaldiroglu.Strategy.Sorting.Problem;

/// <summary>Sorts arrays of three sizes with one class that both chooses the algorithm and contains all three.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Strategy.Demo -- sorting-problem</c>. The random numbers
/// differ from Java's for the same seed, but only the sizes and the sorter names are printed.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var sorter = new Sorter();

        double[] small = [5, 3, 9, 1, 7];
        sorter.Sort(small);
        Console.WriteLine("Sorted " + Show(small) + " with " + sorter.LastUsed);

        foreach (var size in new[] { 5_000, 1_000_000 })
        {
            var list = Shuffled(size);
            sorter.Sort(list);
            Console.WriteLine("Sorted " + size + " numbers with " + sorter.LastUsed);
        }
        Console.WriteLine("One class holds the choice and all three algorithms.");
    }

    private static double[] Shuffled(int size)
    {
        var random = new Random(42);
        var list = new double[size];
        for (var i = 0; i < size; i++) list[i] = random.NextDouble() * 1000;
        return list;
    }

    /// <summary>Prints an array the way Java's <c>Arrays.toString</c> does: <c>[1.0, 3.0]</c>.</summary>
    private static string Show(double[] list) =>
        "[" + string.Join(", ", list.Select(d => d.ToString("0.0###############", CultureInfo.InvariantCulture))) + "]";
}
