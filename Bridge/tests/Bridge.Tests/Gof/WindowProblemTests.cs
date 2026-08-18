using dev.kaldiroglu.Bridge.Gof.Problem;
using Xunit;

namespace dev.kaldiroglu.Bridge.Tests.Gof;

/// <summary>
/// The design GoF start from (Design Patterns, p. 151): the platform is a base class. Every
/// window drawn here is correct; what these tests measure is what it cost.
/// </summary>
public class WindowProblemTests
{
    [Fact(DisplayName = "each platform draws its own way")]
    public void PlatformsDrawDifferently()
    {
        Assert.Equal("+------+\n|      |\n+------+", Window.Render(new XWindow(8, 3)));
        Assert.Equal("#======#\n!      !\n#======#", Window.Render(new PMWindow(8, 3)));
    }

    [Fact(DisplayName = "a window kind must be written once per platform")]
    public void OneClassPerCombination()
    {
        var onX = Window.Render(new XIconWindow(14, 5, "a.txt"));
        var onPm = Window.Render(new PMIconWindow(14, 5, "a.txt"));

        // Same icon layout, drawn by two classes that share no code path.
        Assert.Contains("a.txt", onX);
        Assert.Contains("a.txt", onPm);
        Assert.NotEqual(onX, onPm);
    }

    [Fact(DisplayName = "the platform is the class, so it cannot change once the object exists")]
    public void PlatformIsFixedAtConstruction()
    {
        Window window = new XIconWindow(14, 5, "a.txt");

        // There is no operation that can move this window to Presentation Manager. The only way
        // is to build a different object of a different class — and any state the original held
        // is lost with it.
        Assert.Equal("X", window.Platform);
        Assert.Equal("PM", new PMIconWindow(14, 5, "a.txt").Platform);
        Assert.DoesNotContain(typeof(Window).GetMethods(), m => m.Name.Contains("SetImp"));
    }

    [Fact(DisplayName = "the X drawing code exists three times over")]
    public void PlatformCodeIsDuplicated()
    {
        // XWindow, XIconWindow and XTransientWindow each carry their own copy of the same two
        // methods. Change how X draws a rectangle and you must find all three; the compiler will
        // not tell you which one you missed.
        Assert.Equal("X", new XWindow(8, 3).Platform);
        Assert.Equal("X", new XIconWindow(8, 3, "a").Platform);
        Assert.Equal("X", new XTransientWindow(8, 3, "a").Platform);

        var declaresDrawRect = TypeCensus.In("dev.kaldiroglu.Bridge.Gof.Problem")
            .Count(t => !t.IsAbstract && t.GetMethod("DrawRect")!.DeclaringType == t);
        Assert.Equal(6, declaresDrawRect);   // one per (kind, platform) pair, all six of them
    }

    [Fact(DisplayName = "kinds multiply platforms")]
    public void TheArithmetic()
    {
        var naive = TypeCensus.In("dev.kaldiroglu.Bridge.Gof.Problem");
        var leaves = naive.Count(t => !t.IsAbstract);
        var kinds = naive.Count(t => t.IsAbstract);       // Window, IconWindow, TransientWindow

        Assert.Equal(6, leaves);
        Assert.Equal(3, kinds);                 // plain, icon, transient
        Assert.Equal(leaves, kinds * 2);

        // A third platform is not one class. It is one class per window kind.
        Assert.Equal(9, kinds * 3);
        // A fourth window kind is not one class either.
        Assert.Equal(8, (kinds + 1) * 2);
    }
}
