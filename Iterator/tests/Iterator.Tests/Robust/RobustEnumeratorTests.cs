namespace dev.kaldiroglu.Iterator.Tests.Robust;

using System.Collections.Concurrent;
using dev.kaldiroglu.Iterator.Robust;
using Xunit;

/// <summary>
/// GoF implementation issue 3 (how robust is the iterator?). Every cell of the .NET column on
/// the robust-iterator slide is asserted here, against the base class library itself.
/// </summary>
public class RobustEnumeratorTests
{
    private static Dictionary<string, int> Abc() => new() { ["A"] = 1, ["B"] = 2, ["C"] = 3 };

    [Fact(DisplayName = "List<T> fails: adding during the walk throws InvalidOperationException")]
    public void ListFailsOnAdd()
    {
        var list = new List<string> { "A", "B", "C" };
        Assert.Equal("InvalidOperationException after [A]",
            Walk.ChangedAfterFirst(list, () => list.Add("D")));
    }

    [Fact(DisplayName = "Dictionary fails on Add, but not on Remove")]
    public void DictionaryFailsOnAddNotOnRemove()
    {
        var added = Abc();
        Assert.Equal("InvalidOperationException after [A]",
            Walk.ChangedAfterFirst(added.Keys, () => added.Add("D", 4)));
        var removed = Abc();
        Assert.Equal("no exception, saw [A, B]",
            Walk.ChangedAfterFirst(removed.Keys, () => removed.Remove("C")));
    }

    [Fact(DisplayName = "ConcurrentQueue ignores the change: it walks a snapshot")]
    public void ConcurrentQueueIgnoresTheChange()
    {
        var queue = new ConcurrentQueue<string>(["A", "B", "C"]);
        Assert.Equal("no exception, saw [A, B, C]",
            Walk.ChangedAfterFirst(queue, () => queue.Enqueue("D")));
        Assert.Equal(["A", "B", "C", "D"], queue.ToArray());
    }

    [Fact(DisplayName = "ConcurrentDictionary never fails, and the walk still reads the original elements")]
    public void ConcurrentDictionaryNeverFails()
    {
        var map = new ConcurrentDictionary<string, int>(Abc());
        var outcome = Walk.ChangedAfterFirst(map.Select(pair => pair.Key), () => map["D"] = 4);
        Assert.StartsWith("no exception", outcome);
        Assert.Contains("A", outcome);
        Assert.Contains("B", outcome);
        Assert.Contains("C", outcome);
    }

    [Fact(DisplayName = "replacing an element of a List<T> is a change: the next step fails")]
    public void ListSetFails()
    {
        var list = new List<string> { "A", "B", "C" };
        Assert.Equal("InvalidOperationException after [A]",
            Walk.ChangedAfterFirst(list, () => list[2] = "Z"));
    }

    [Fact(DisplayName = "an array never fails, and the walk sees the replaced element")]
    public void ArraySeesTheChange()
    {
        string[] array = ["A", "B", "C"];
        Assert.Equal("no exception, saw [A, B, Z]",
            Walk.ChangedAfterFirst(array, () => array[2] = "Z"));
    }

    [Fact(DisplayName = "RemoveAll changes the list without a walk that can fail")]
    public void RemoveAllIsSafe()
    {
        var list = new List<string> { "A", "B", "C" };
        list.RemoveAll(element => element == "B");
        Assert.Equal(["A", "C"], list);
    }

    [Fact(DisplayName = "walking a copy lets the original change without failing")]
    public void WalkingACopyIsSafe()
    {
        var list = new List<string> { "A", "B", "C" };
        Assert.Equal("no exception, saw [A, B, C]",
            Walk.ChangedAfterFirst(list.ToList(), () => list.Add("D")));
    }

    [Fact(DisplayName = "Main prints the outcomes the slide quotes")]
    public void MainPrintsTheOutcomes()
    {
        Assert.Equal([
            "Fails:",
            "  List<T>, Add D: InvalidOperationException after [A]",
            "  Dictionary, Add D: InvalidOperationException after [A]",
            "Ignores the change:",
            "  ConcurrentQueue<T>, Enqueue D: no exception, saw [A, B, C]",
            "May see the change, and never fails:",
            "  ConcurrentDictionary, add D: no exception",
            "Replacing an element:",
            "  List<T>, set C to Z: InvalidOperationException after [A]",
            "  array, set C to Z: no exception, saw [A, B, Z]"],
            Printed.By(dev.kaldiroglu.Iterator.Robust.Main.Run));
    }
}
