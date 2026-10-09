namespace dev.kaldiroglu.Iterator.Tests.OrgChart.Solution;

using System.Collections;
using System.Reflection;
using dev.kaldiroglu.Iterator.OrgChart.Domain;
using dev.kaldiroglu.Iterator.OrgChart.Solution;
using Xunit;

/// <summary>
/// The department that gives out iterators. Every output the Part 3 slides quote from
/// <c>OrgChart.Solution.Main</c> is asserted here. Ported from the Java
/// <c>orgchart.solution.SolutionTest</c>.
/// </summary>
public class SolutionTests
{
    private static List<string> Walk(IEnumerable<Employee> employees)
    {
        var names = new List<string>();
        foreach (var employee in employees)
        {
            names.Add(employee.ToString());
        }
        return names;
    }

    /// <summary>Whether a type is a list: a non-generic IList, or a generic list or collection.</summary>
    private static bool IsList(Type type)
    {
        if (typeof(IList).IsAssignableFrom(type))
        {
            return true;
        }
        Type[] listTypes = [typeof(IList<>), typeof(IReadOnlyList<>), typeof(ICollection<>), typeof(IReadOnlyCollection<>)];
        return type.GetInterfaces().Append(type)
            .Any(t => t.IsGenericType && listTypes.Contains(t.GetGenericTypeDefinition()));
    }

    [Fact(DisplayName = "Main prints the two orders and the first change, as the Part 3 slides quote them")]
    public void MainPrintsWhatTheSlidesQuote()
    {
        var lines = Printed.By(Main.Run);

        Assert.Equal([
            "Department by department:",
            "  Ayse (CEO)",
            "  Deniz (head of sales)",
            "  Ali (sales)",
            "  Can (export)",
            "  Mert (head of operations)",
            "  Elif (support)",
            "Level by level:",
            "  Ayse (CEO)",
            "  Deniz (head of sales)",
            "  Ali (sales)",
            "  Mert (head of operations)",
            "  Can (export)",
            "  Elif (support)",
            "First change: Deniz (head of sales) -> Zeynep (head of sales)"], lines);
    }

    [Fact(DisplayName = "the two orders differ only in rows 4 and 5: Can is one level lower than Mert")]
    public void TheTwoOrdersDifferInRowsFourAndFive()
    {
        var company = Main.Company("Deniz");

        var byDepartment = Walk(company);
        var byLevel = Walk(company.ByLevel());

        Assert.Equal(6, byDepartment.Count);
        Assert.Equal(6, byLevel.Count);
        for (var row = 1; row <= 6; row++)
        {
            var same = byDepartment[row - 1] == byLevel[row - 1];
            Assert.True(same == (row != 4 && row != 5), "row " + row);
        }
        Assert.Equal("Can (export)", byDepartment[3]);
        Assert.Equal("Mert (head of operations)", byLevel[3]);
    }

    [Fact(DisplayName = "the change report moves two iterators together and stops at the first difference")]
    public void TheReportFindsTheNewHeadOfSales()
    {
        // Java returns Optional<String>; the C# port returns string?, with null for none.
        var change = new ChangeReport().FirstDifference(Main.Company("Deniz"), Main.Company("Zeynep"));

        Assert.Equal("Deniz (head of sales) -> Zeynep (head of sales)", change);
    }

    [Fact(DisplayName = "the change report answers none for equal charts and sees a size change")]
    public void TheReportHandlesEqualAndShorterCharts()
    {
        var shorter = new Department("Head office").Add(new Employee("Ayse", "CEO"));

        Assert.Null(new ChangeReport().FirstDifference(Main.Company("Deniz"), Main.Company("Deniz")));
        Assert.Equal("the charts have different sizes",
            new ChangeReport().FirstDifference(Main.Company("Deniz"), shorter));
        Assert.Equal("the charts have different sizes",
            new ChangeReport().FirstDifference(shorter, Main.Company("Deniz")));
    }

    [Fact(DisplayName = "two iterators over the same department keep their own positions")]
    public void TwoIteratorsDoNotDisturbEachOther()
    {
        var company = Main.Company("Deniz");
        using var first = company.GetEnumerator();
        using var second = company.GetEnumerator();

        // Java's next() moves on and returns the element; C#'s MoveNext() moves on and
        // Current returns it.
        first.MoveNext();
        first.MoveNext();
        first.MoveNext();

        Assert.True(second.MoveNext());
        Assert.Equal("Ayse (CEO)", second.Current.ToString());
        Assert.True(first.MoveNext());
        Assert.Equal("Can (export)", first.Current.ToString());
    }

    [Fact(DisplayName = "the department has no public getter for its members or sub-departments")]
    public void TheDepartmentGivesOutNoLists()
    {
        // Java looks at public methods that return a List. In C# a getter is a property, so
        // this looks at public methods and public properties that return a list type.
        const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static
                                   | BindingFlags.DeclaredOnly;
        var listGetters = typeof(Department).GetMethods(flags)
            .Where(m => IsList(m.ReturnType)).Select(m => m.Name)
            .Concat(typeof(Department).GetProperties(flags)
                .Where(p => IsList(p.PropertyType)).Select(p => p.Name))
            .ToList();

        Assert.Empty(listGetters);
    }

    [Fact(DisplayName = "nothing is copied: a person added to a department the walk has not reached yet is seen")]
    public void TheIteratorReadsTheLiveDepartment()
    {
        var later = new Department("Later").Add(new Employee("Elif", "support"));
        var company = new Department("Head office")
            .Add(new Employee("Ayse", "CEO"))
            .Add(later);
        using var walk = company.GetEnumerator();

        Assert.True(walk.MoveNext());
        Assert.Equal("Ayse (CEO)", walk.Current.ToString());
        later.Add(new Employee("Mert", "support"));

        Assert.True(walk.MoveNext());
        Assert.Equal("Elif (support)", walk.Current.ToString());
        Assert.True(walk.MoveNext());
        Assert.Equal("Mert (support)", walk.Current.ToString());
        Assert.False(walk.MoveNext());
    }

    [Fact(DisplayName = "adding to the list the iterator is walking throws ConcurrentModificationException on the next step")]
    public void AddingDuringTheWalkThrows()
    {
        var sales = new Department("Sales")
            .Add(new Employee("Deniz", "head of sales"))
            .Add(new Employee("Ali", "sales"));
        using var walk = sales.GetEnumerator();
        walk.MoveNext();

        sales.Add(new Employee("Can", "sales"));

        // Java throws ConcurrentModificationException. A .NET List<T> enumerator throws
        // InvalidOperationException when its list changes during the walk.
        Assert.Throws<InvalidOperationException>(() => walk.MoveNext());
    }

    [Fact(DisplayName = "an iterator past the last person throws NoSuchElementException, in both orders")]
    public void BothIteratorsEndCleanly()
    {
        var empty = new Department("Empty").Add(new Department("Also empty"));

        // Java's next() past the end throws NoSuchElementException. In C# MoveNext() answers
        // false, and Current then throws InvalidOperationException.
        using var depthFirst = empty.GetEnumerator();
        Assert.False(depthFirst.MoveNext());
        Assert.Throws<InvalidOperationException>(() => depthFirst.Current);

        using var byLevel = empty.ByLevel().GetEnumerator();
        Assert.False(byLevel.MoveNext());
        Assert.Throws<InvalidOperationException>(() => byLevel.Current);
    }
}
