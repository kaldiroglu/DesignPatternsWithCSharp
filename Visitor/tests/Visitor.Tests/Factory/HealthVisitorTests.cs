using Xunit;
using F = global::dev.kaldiroglu.Visitor.Factory;

namespace dev.kaldiroglu.Visitor.Tests.Factory;

/// <summary>
/// The health check: everyone who has worked more than five years, every manager's
/// psychological state, and the boss if the boss is over fifty.
/// </summary>
public class HealthVisitorTests
{
    [Fact]
    public void MoreThanFiveYears()
    {
        var visitor = new F.HealthVisitor();

        Assert.Equal(["Checking the health status of employee: 1 Ayse"],
            Printed.By(() => new F.Employee(1, "Ayse", 6, "Sales").Accept(visitor)));
        Assert.Empty(Printed.By(() => new F.Engineer(2, "Burhan", 5, "Production", "p").Accept(visitor)));
    }

    [Fact]
    public void Managers()
    {
        var visitor = new F.HealthVisitor();

        Assert.Equal(["Checking the psychological status of manager: 4 Metin",
                      "Checking the health status of employee: 4 Metin"],
            Printed.By(() => new F.Manager(4, "Metin", 14, "Production", "Production").Accept(visitor)));
        Assert.Equal(["Checking the psychological status of manager: 5 Salih"],
            Printed.By(() => new F.Director(5, "Salih", 3, "Management", "Management", 4500).Accept(visitor)));
    }

    [Fact]
    public void TheBoss()
    {
        var visitor = new F.HealthVisitor();

        Assert.Equal(["Checking the health status of boss: Memet Emmi"],
            Printed.By(() => new F.Boss("Memet Emmi", 52).Accept(visitor)));
        Assert.Empty(Printed.By(() => new F.Boss("Young", 50).Accept(visitor)));
    }

    [Fact]
    public void OneVisitForFourClasses()
    {
        Assert.Equal(typeof(object), typeof(F.Boss).BaseType);
        Assert.False(typeof(F.Employee).IsAssignableFrom(typeof(F.Boss)));

        Type[] subclasses = [typeof(F.Engineer), typeof(F.Secretary), typeof(F.Manager), typeof(F.Director)];
        Assert.All(subclasses, t => Assert.True(typeof(F.Employee).IsAssignableFrom(t), t.Name));
        Assert.True(typeof(F.Manager).IsAssignableFrom(typeof(F.Director)));

        var methods = typeof(F.IVisitor).GetMethods();
        var visited = methods.Select(m => m.GetParameters()[0].ParameterType).ToList();
        Assert.Equal(2, visited.Count);
        Assert.Contains(typeof(F.Employee), visited);
        Assert.Contains(typeof(F.Boss), visited);
        Assert.All(methods, m => Assert.Equal("Visit", m.Name));
    }
}
