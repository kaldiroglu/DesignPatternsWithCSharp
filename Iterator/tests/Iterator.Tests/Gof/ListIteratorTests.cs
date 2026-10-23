namespace dev.kaldiroglu.Iterator.Tests.Gof;

// The usings sit inside the namespace. GoF's list is named List<T>, as .NET's is; the alias
// GofList names it apart, and the plain List<T> in this file is .NET's.
using dev.kaldiroglu.Iterator.Gof;
using dev.kaldiroglu.Iterator.Gof.Problem;
using dev.kaldiroglu.Iterator.Gof.Solution;
using Xunit;
using GofList = dev.kaldiroglu.Iterator.Gof.Solution.List<dev.kaldiroglu.Iterator.Gof.Employee>;
using List = System.Collections.Generic.List<string>;
using ProblemMain = dev.kaldiroglu.Iterator.Gof.Problem.Main;

/// <summary>
/// GoF's list example. The 3 pairs and the 9 pairs on the Part 2 slides are asserted here,
/// from the lists themselves and from the output of the runnable classes: <c>Problem.Main</c>,
/// <c>TwoIteratorsMain</c>, <c>PolymorphicIterationMain</c> and <c>InternalIteratorMain</c>.
/// </summary>
public class ListIteratorTests
{
    private static readonly string[] Names = ["Ayse", "Deniz", "Elif"];

    private static TList Filled<TList>(TList list) where TList : AbstractList<Employee>
    {
        foreach (var name in Names)
        {
            list.Append(new Employee(name));
        }
        return list;
    }

    [Fact(DisplayName = "a list that walks itself: a loop inside a loop gives 3 pairs, not 9")]
    public void OneCursorGivesThreePairs()
    {
        var list = new CursorList<Employee>();
        foreach (var name in Names)
        {
            list.Append(new Employee(name));
        }

        var pairs = new List();
        for (list.First(); !list.IsDone(); list.Next())
        {
            var a = list.CurrentItem();
            for (list.First(); !list.IsDone(); list.Next())
            {
                pairs.Add(a + "-" + list.CurrentItem());
            }
        }

        Assert.Equal(3, Names.Length);
        Assert.Equal(3, pairs.Count);
        Assert.Equal(["Ayse-Ayse", "Ayse-Deniz", "Ayse-Elif"], pairs);
    }

    [Fact(DisplayName = "with two iterators, the loop inside a loop gives all 9 pairs")]
    public void TwoIteratorsGiveNinePairs()
    {
        var list = Filled(new GofList());

        var pairs = new List();
        var outer = list.CreateIterator();
        for (outer.First(); !outer.IsDone(); outer.Next())
        {
            var inner = list.CreateIterator();
            for (inner.First(); !inner.IsDone(); inner.Next())
            {
                pairs.Add(outer.CurrentItem() + "-" + inner.CurrentItem());
            }
        }

        Assert.Equal(Names.Length * Names.Length, pairs.Count);
        Assert.Equal(9, pairs.Count);
    }

    [Fact(DisplayName = "problem.Main prints 3 pairs, because the inner loop moved the only cursor")]
    public void ProblemMainPrintsThreePairs()
    {
        Assert.Equal([
            "Expected 9 pairs, got 3: [Ayse-Ayse, Ayse-Deniz, Ayse-Elif]",
            "The inner loop moved the only cursor, so the outer loop ended after Ayse."],
            Printed.By(ProblemMain.Run));
    }

    [Fact(DisplayName = "TwoIteratorsMain prints all 9 pairs")]
    public void TwoIteratorsMainPrintsNinePairs()
    {
        Assert.Equal([
            "Two iterators, 9 pairs: [Ayse-Ayse, Ayse-Deniz, Ayse-Elif, Deniz-Ayse, "
            + "Deniz-Deniz, Deniz-Elif, Elif-Ayse, Elif-Deniz, Elif-Elif]"],
            Printed.By(TwoIteratorsMain.Run));
    }

    [Fact(DisplayName = "PolymorphicIterationMain walks a list backwards and walks a chain list")]
    public void PolymorphicIterationMainPrintsBothWalks()
    {
        Assert.Equal([
            "Backward: [Elif, Deniz, Ayse]",
            "Chain list: [Ayse, Deniz, Elif]"],
            Printed.By(PolymorphicIterationMain.Run));
    }

    [Fact(DisplayName = "InternalIteratorMain stops after two employees")]
    public void InternalIteratorMainStopsAfterTwo()
    {
        Assert.Equal(["First two: [Ayse, Deniz], walked to the end: false"],
            Printed.By(InternalIteratorMain.Run));
    }

    [Fact(DisplayName = "the same client walks an array list and a chain list without knowing which it has")]
    public void PolymorphicIteration()
    {
        AbstractList<Employee>[] lists = [Filled(new GofList()), Filled(new ChainList<Employee>())];

        foreach (var list in lists)
        {
            Assert.Equal(["Ayse", "Deniz", "Elif"], PrintEmployees.Print(list.CreateIterator()));
        }
    }

    [Fact(DisplayName = "a reverse iterator walks the same list backwards, and the list did not change")]
    public void ReverseIterator()
    {
        var list = Filled(new GofList());

        Assert.Equal(["Elif", "Deniz", "Ayse"], PrintEmployees.Print(new ReverseListIterator<Employee>(list)));
        Assert.Equal(["Ayse", "Deniz", "Elif"], PrintEmployees.Print(list.CreateIterator()));
    }

    [Fact(DisplayName = "an iterator past the end refuses to give an item")]
    public void CurrentItemAfterTheEnd()
    {
        var list = Filled(new GofList());
        var forward = list.CreateIterator();
        var chain = Filled(new ChainList<Employee>()).CreateIterator();
        IIterator<Employee> backward = new ReverseListIterator<Employee>(list);

        foreach (var iterator in new[] { forward, chain, backward })
        {
            for (iterator.First(); !iterator.IsDone(); iterator.Next())
            {
                iterator.CurrentItem();
            }
            // Past the end, CurrentItem() throws InvalidOperationException.
            Assert.Throws<InvalidOperationException>(() => iterator.CurrentItem());
        }
    }

    [Fact(DisplayName = "the internal iterator stops early when processItem returns false")]
    public void PrintNEmployeesStopsAfterN()
    {
        var firstTwo = new PrintNEmployees(Filled(new GofList()), 2);

        Assert.False(firstTwo.Traverse(), "the walk stopped early");
        Assert.Equal(["Ayse", "Deniz"], firstTwo.Lines);
    }

    /// <summary>
    /// A traverser that never stops. C# has no anonymous classes, so it is a nested class.
    /// </summary>
    private sealed class SeeEveryone(AbstractList<Employee> list, List seen) : ListTraverser<Employee>(list)
    {
        protected override bool ProcessItem(Employee item)
        {
            seen.Add(item.Name);
            return true;
        }
    }

    [Fact(DisplayName = "the internal iterator answers true when it processed every item")]
    public void ATraverserThatNeverStops()
    {
        var seen = new List();
        var all = new SeeEveryone(Filled(new ChainList<Employee>()), seen);

        Assert.True(all.Traverse());
        Assert.Equal(["Ayse", "Deniz", "Elif"], seen);
    }

    [Fact(DisplayName = "lists grow past their first four slots")]
    public void ListsGrow()
    {
        var list = new GofList();
        var cursorList = new CursorList<Employee>();
        for (var i = 0; i < 9; i++)
        {
            list.Append(new Employee("E" + i));
            cursorList.Append(new Employee("E" + i));
        }

        Assert.Equal(9, list.Count);
        Assert.Equal(9, cursorList.Count);
        Assert.Equal("E8", list.Get(8).Name);
        // An index past the end throws ArgumentOutOfRangeException.
        Assert.Throws<ArgumentOutOfRangeException>(() => list.Get(9));
    }
}
