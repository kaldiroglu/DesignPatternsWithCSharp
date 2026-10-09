using System.Globalization;

namespace dev.kaldiroglu.Strategy.Sorting.Subclassing;

/// <summary>Sorts one array with each subclass, and shows that the caller now has to choose which one.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Strategy.Demo -- sorting-subclassing</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        Sorter[] sorters = [new BubbleSorter(), new QuickSorter(), new JavaSorter()];
        foreach (var sorter in sorters)
        {
            double[] list = [5, 3, 9, 1, 7];
            sorter.Sort(list);
            Console.WriteLine(sorter.Name + ": " + Show(list));
        }
        Console.WriteLine("Each algorithm is its own class, and nothing here chooses by size.");
        Console.WriteLine("Every caller has to know which class suits which array.");
    }

    /// <summary>Prints an array the way Java's <c>Arrays.toString</c> does: <c>[1.0, 3.0]</c>.</summary>
    private static string Show(double[] list) =>
        "[" + string.Join(", ", list.Select(d => d.ToString("0.0###############", CultureInfo.InvariantCulture))) + "]";
}
