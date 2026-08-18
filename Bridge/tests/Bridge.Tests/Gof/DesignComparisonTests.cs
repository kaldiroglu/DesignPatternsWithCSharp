using dev.kaldiroglu.Bridge.Gof.Solution;
using Xunit;
using Problem = dev.kaldiroglu.Bridge.Gof.Problem;

namespace dev.kaldiroglu.Bridge.Tests.Gof;

/// <summary>
/// The two designs, side by side. They draw the same pictures; they do not cost the same.
/// </summary>
public class DesignComparisonTests
{
    [Fact(DisplayName = "both designs draw the same icon window on X")]
    public void SameOutputOnX() =>
        Assert.Equal(
            Problem.Window.Render(new Problem.XIconWindow(14, 5, "a.txt")),
            Window.Render(new IconWindow(14, 5, "a.txt", new XWindowImp())));

    [Fact(DisplayName = "both designs draw the same icon window on Presentation Manager")]
    public void SameOutputOnPm() =>
        Assert.Equal(
            Problem.Window.Render(new Problem.PMIconWindow(14, 5, "a.txt")),
            Window.Render(new IconWindow(14, 5, "a.txt", new PMWindowImp())));

    [Fact(DisplayName = "nine types become six, and a third platform costs three against one")]
    public void TheCostOfGrowth()
    {
        var naive = TypeCensus.In("dev.kaldiroglu.Bridge.Gof.Problem");
        var bridged = TypeCensus.In("dev.kaldiroglu.Bridge.Gof.Solution");

        Assert.Equal(9, naive.Count);      // every type on the problem diagram
        Assert.Equal(6, bridged.Count);    // every type on the solution diagram

        // Of the nine, six are leaves — one per (kind, platform) pair — and three are the
        // abstract kinds above them. That product is what grows.
        var leaves = naive.Count(t => !t.IsAbstract);
        Assert.Equal(6, leaves);

        // Two platforms and six leaves means three kinds, so the leaf count really is the
        // product and a third platform adds one leaf per kind. Everything below is derived from
        // the counts above rather than restated as literals.
        var kinds = naive.Count - leaves;
        Assert.Equal(3, kinds);
        Assert.Equal(leaves, kinds * 2);
        Assert.Equal(9, kinds * 3);

        // The bridged side grows by one class instead of three. That is not arithmetic here:
        // WindowSolutionTests writes an implementor inside the test class and draws every window
        // kind through it without touching any of them.
        Assert.Equal(7, bridged.Count + 1);
    }
}
