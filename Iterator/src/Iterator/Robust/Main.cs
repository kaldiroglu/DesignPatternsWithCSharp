using System.Collections.Concurrent;

namespace dev.kaldiroglu.Iterator.Robust;

/// <summary>
/// GoF implementation issue 3 (how robust is the iterator?): what the base class library's
/// enumerators do when the collection changes during the walk. Each walk reads A, B and C,
/// and the collection changes after A.
/// </summary>
/// <remarks>
/// Run it with <c>dotnet run --project src/Iterator.Demo -- robust</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        Console.WriteLine("Fails:");
        var list = new List<string> { "A", "B", "C" };
        Console.WriteLine("  List<T>, Add D: "
            + Walk.ChangedAfterFirst(list, () => list.Add("D")));
        var dictionary = Abc();
        Console.WriteLine("  Dictionary, Add D: "
            + Walk.ChangedAfterFirst(dictionary.Keys, () => dictionary.Add("D", 4)));

        Console.WriteLine("Ignores the change:");
        var queue = new ConcurrentQueue<string>(["A", "B", "C"]);
        Console.WriteLine("  ConcurrentQueue<T>, Enqueue D: "
            + Walk.ChangedAfterFirst(queue, () => queue.Enqueue("D")));

        Console.WriteLine("May see the change, and never fails:");
        var concurrent = new ConcurrentDictionary<string, int>(Abc());
        var outcome = Walk.ChangedAfterFirst(concurrent.Select(pair => pair.Key),
            () => concurrent["D"] = 4);
        Console.WriteLine("  ConcurrentDictionary, add D: "
            + (outcome.StartsWith("no exception") ? "no exception" : outcome));

        Console.WriteLine("Replacing an element:");
        var replaced = new List<string> { "A", "B", "C" };
        Console.WriteLine("  List<T>, set C to Z: "
            + Walk.ChangedAfterFirst(replaced, () => replaced[2] = "Z"));
        string[] array = ["A", "B", "C"];
        Console.WriteLine("  array, set C to Z: "
            + Walk.ChangedAfterFirst(array, () => array[2] = "Z"));
    }

    private static Dictionary<string, int> Abc() => new() { ["A"] = 1, ["B"] = 2, ["C"] = 3 };
}
