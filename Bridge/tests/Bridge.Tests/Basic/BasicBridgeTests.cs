using System.Reflection;
using Xunit;
using Pattern = dev.kaldiroglu.Bridge.Basic.Pattern;
using Problem = dev.kaldiroglu.Bridge.Basic.Problem;

namespace dev.kaldiroglu.Bridge.Tests.Basic;

/// <summary>
/// The solution reduced to its bones, and the counting argument that justifies it.
/// </summary>
public class BasicBridgeTests
{
    private const string PatternNs = "dev.kaldiroglu.Bridge.Basic.Pattern";
    private const string ProblemNs = "dev.kaldiroglu.Bridge.Basic.Problem";

    [Fact(DisplayName = "every refinement works with every implementation, from four classes")]
    public void MPlusN()
    {
        var refinements = TypeCensus.ConcreteImplementationsOf(
            PatternNs, typeof(Pattern.IAnAbstraction));
        var implementations = TypeCensus.ConcreteImplementationsOf(
            PatternNs, typeof(Pattern.IAnAbstractionImplementation));

        Assert.Equal(2, refinements);
        Assert.Equal(2, implementations);
        Assert.Equal(4, refinements + implementations);
        Assert.Equal(4, refinements * implementations); // 2x2 happens to match
    }

    [Fact(DisplayName = "the naive namespace needs a class per combination")]
    public void MTimesN()
    {
        // Leaves are the concrete cells of the grid: every type whose base class is itself a
        // refinement. Counted, not listed, so a fourth leaf breaks this rather than the slide.
        var leaves = TypeCensus.In(ProblemNs)
            .Count(t => t.BaseType is not null
                        && typeof(Problem.IAnAbstraction).IsAssignableFrom(t.BaseType));

        Assert.Equal(4, leaves);
        Assert.Equal(6, leaves + 2); // plus the two refinements they extend
    }

    [Fact(DisplayName = "in the solution the refinement holds an implementation; in the problem it is one")]
    public void HeldAgainstInherited()
    {
        Assert.Single(typeof(Pattern.ASubAbstraction)
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic));

        // The naive refinement has no field for an implementation, because it *is* the
        // implementation's base class — which is exactly why it cannot switch.
        Assert.Empty(typeof(Problem.ASubAbstraction)
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic));
        Assert.True(typeof(Problem.ASubAbstraction)
            .IsAssignableFrom(typeof(Problem.AConcreteImplementation1)));
    }

    [Fact(DisplayName = "the implementor interface is spelled correctly")]
    public void TheInterfaceNameIsSpelledCorrectly()
    {
        // It was AnAbstrationImplementation for a long time. Names on slides get read aloud.
        Assert.Equal("IAnAbstractionImplementation",
            typeof(Pattern.IAnAbstractionImplementation).Name);
        Assert.DoesNotContain("Abstration", typeof(Pattern.IAnAbstractionImplementation).Name);
    }

    [Fact(DisplayName = "the client is identical in both designs")]
    public void TheClientDoesNotChange()
    {
        // Whatever else differs, it is not the calling code. The client holds the abstraction
        // and calls one method, in both namespaces — which is why the class count below is the
        // only thing the two designs argue about.
        Assert.Empty(typeof(Pattern.Client).GetMethod(nameof(Pattern.Client.Start))!.GetParameters());
        Assert.Empty(typeof(Problem.Client).GetMethod(nameof(Problem.Client.Start))!.GetParameters());

        Assert.Equal(typeof(Pattern.IAnAbstraction),
            typeof(Pattern.Client).GetConstructors()[0].GetParameters()[0].ParameterType);
        Assert.Equal(typeof(Problem.IAnAbstraction),
            typeof(Problem.Client).GetConstructors()[0].GetParameters()[0].ParameterType);
    }
}
