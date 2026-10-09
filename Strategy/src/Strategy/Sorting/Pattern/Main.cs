using System.Globalization;

namespace dev.kaldiroglu.Strategy.Sorting.Pattern;

/// <summary>Sorts arrays of three sizes through the context, which selects a sorter and implements none.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Strategy.Demo -- sorting-pattern</c>. The random numbers
/// differ from Java's for the same seed, but only the sizes and the sorter names are printed.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var context = new SortingContext();

        double[] small = [5, 3, 9, 1, 7];
        context.Sort(small);
        Console.WriteLine("Sorted " + Show(small) + " with " + context.LastUsed);

        foreach (var size in new[] { 5_000, 1_000_000 })
        {
            var list = Shuffled(size);
            context.Sort(list);
            Console.WriteLine("Sorted " + size + " numbers with " + context.LastUsed);
        }
        Console.WriteLine("For a billion numbers the context would choose "
            + context.SorterFor(1_000_000_000).Name + ", and nothing was sorted to find out.");
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
