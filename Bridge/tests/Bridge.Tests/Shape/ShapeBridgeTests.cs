using Xunit;
using Problem = dev.kaldiroglu.Bridge.Shape.Problem;
using dev.kaldiroglu.Bridge.Shape.Solution;

namespace dev.kaldiroglu.Bridge.Tests.Shape;

/// <summary>
/// Shapes over two window systems: the same object, drawn two ways, and the device rebuilding
/// what it cannot draw.
/// </summary>
public class ShapeBridgeTests
{
    [Fact(DisplayName = "one shape over two devices — and the device decides how, not what")]
    public void OneShapeTwoDevices()
    {
        var mac = new MacOSDrawer();
        var x = new XWindowsDrawer();

        new Circle("c", mac, 50, 50, 20).Draw();
        new Circle("c", x, 50, 50, 20).Draw();

        // MacOS draws arcs natively; XWindows has no arc call and builds one from segments.
        Assert.Single(mac.Calls);
        Assert.Equal(16, x.Calls.Count);
        Assert.StartsWith("arc", mac.Calls[0]);
        Assert.All(x.Calls, call => Assert.StartsWith("line", call));
    }

    [Fact(DisplayName = "the same shape object moves between devices at run time")]
    public void TheDeviceCanChangeOnALiveObject()
    {
        var mac = new MacOSDrawer();
        var x = new XWindowsDrawer();
        IShape circle = new Circle("c", mac, 50, 50, 20);

        circle.Draw();
        Assert.Single(mac.Calls);

        circle.SetDrawer(x);
        circle.Draw();
        Assert.Equal(16, x.Calls.Count);
        Assert.Single(mac.Calls); // nothing more went to the old device
    }

    [Fact(DisplayName = "each shape composes the primitives its own way")]
    public void EveryShapeComposesPrimitives()
    {
        var mac = new MacOSDrawer();

        new Rectangle("r", mac, 10, 10, 40, 20).Draw();
        Assert.Equal(4, mac.Calls.Count);

        mac.ResetCalls();
        new Triangle("t", mac, 0, 0, 20, 0, 10, 15).Draw();
        Assert.Equal(3, mac.Calls.Count);

        mac.ResetCalls();
        new Circle("c", mac, 0, 0, 5).Erase();
        Assert.Single(mac.Calls);
        Assert.StartsWith("clear", mac.Calls[0]);
    }

    [Fact(DisplayName = "adding a shape cost one class and no drawer changed")]
    public void ANewShapeCostsOneClass()
    {
        // Triangle was written after both drawers existed. If IShapeDrawer had carried a method
        // per shape, this class could not have been added without editing both.
        Assert.Equal(3, typeof(IShapeDrawer).GetMethods().Length);
        Assert.All(new[] { typeof(Circle), typeof(Rectangle), typeof(Triangle) },
            t => Assert.True(typeof(IShape).IsAssignableFrom(t)));
    }

    [Fact(DisplayName = "no primitive names a shape")]
    public void TheImplementorKnowsNothingAboutShapes()
    {
        string[] shapeWords = ["circle", "rectangle", "triangle", "square", "shape"];

        foreach (var method in typeof(IShapeDrawer).GetMethods())
        {
            var name = method.Name.ToLowerInvariant();
            Assert.False(shapeWords.Any(name.Contains),
                $"IShapeDrawer.{method.Name} names a shape, so the two hierarchies are no longer "
                + "independent — a new shape would force every drawer to change");
        }
    }

    [Fact(DisplayName = "the naive design binds the device to the class instead")]
    public void TheProblemNamespaceCannotSwitch()
    {
        var macCircle = new Problem.CircleMacOS("c");
        var xCircle = new Problem.CircleXWindows("c");

        // Two objects, two classes, for one shape on two devices — and no SetDrawer anywhere.
        Assert.NotEqual(macCircle.GetType(), xCircle.GetType());
        Assert.DoesNotContain(typeof(Problem.IShape).GetMethods(), m => m.Name == "SetDrawer");
    }

    [Fact(DisplayName = "nine types become nine against seven, and the gap widens")]
    public void TheCostOfGrowth()
    {
        var naive = TypeCensus.In("dev.kaldiroglu.Bridge.Shape.Problem");
        var bridged = TypeCensus.In("dev.kaldiroglu.Bridge.Shape.Solution");

        var leaves = naive.Count(t => !t.IsAbstract && !t.IsInterface);
        // An interface reports IsAbstract == true in .NET, so IShape has to be excluded by hand.
        var kinds = naive.Count(t => t.IsAbstract && !t.IsInterface
                                     && t != typeof(Problem.AbstractShape));

        Assert.Equal(6, leaves);                      // one per (kind, device) pair
        Assert.Equal(3, kinds);                       // circle, rectangle, triangle
        Assert.Equal(leaves, kinds * 2);              // leaves really are kinds x devices
        Assert.Equal(9, kinds * 3);                   // leaves after a third device

        var shapes = TypeCensus.ConcreteImplementationsOf(
            "dev.kaldiroglu.Bridge.Shape.Solution", typeof(IShape));
        var drawers = TypeCensus.ConcreteImplementationsOf(
            "dev.kaldiroglu.Bridge.Shape.Solution", typeof(IShapeDrawer));
        Assert.Equal(3, shapes);
        Assert.Equal(2, drawers);
        Assert.Equal(5, shapes + drawers);            // m + n against the six leaves above
        Assert.Equal(6, shapes * drawers);            // m x n, what the leaves opposite cost

        // Whole-namespace totals, so a third device can be priced. The naive side grows by one
        // leaf per kind — and `kinds` is counted above, not written here — where the bridged
        // side grows by exactly one drawer.
        Assert.Equal(11, naive.Count);
        Assert.Equal(9, bridged.Count);
        Assert.Equal(14, naive.Count + kinds);
        Assert.Equal(10, bridged.Count + 1);
    }
}
