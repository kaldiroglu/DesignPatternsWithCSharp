using dev.kaldiroglu.Composite.Bom.Domain;
using dev.kaldiroglu.Composite.Bom.Solution;
using Xunit;

namespace dev.kaldiroglu.Composite.Tests.Bom;

/// <summary>
/// The roll-up arithmetic: cost, weight and part count, at every level of the
/// sample bicycle.
/// </summary>
public class RollUpTests
{
    private readonly ProductCatalog.BicycleStructure _catalog = ProductCatalog.CityBicycle();

    [Fact(DisplayName = "A purchased part answers from its own two fields")]
    public void APartAnswersForItself()
    {
        var spoke = _catalog.Spoke;

        Assert.Equal(Money.Of(0.40m), spoke.TotalCost());
        Assert.Equal(5, spoke.TotalWeightGrams());
        Assert.Equal(1, spoke.PartCount());
        Assert.False(spoke.IsAssembly);
    }

    [Fact(DisplayName = "The innermost sub-assembly adds its own cost to its lines")]
    public void HubRollsUp()
    {
        var hub = _catalog.Hub;

        // labor 3.00 + axle 6.50 + 2 bearings at 2.10
        Assert.Equal(Money.Of(13.70m), hub.TotalCost());
        // own 20 g + axle 120 g + 2 x 15 g
        Assert.Equal(170, hub.TotalWeightGrams());
        Assert.Equal(3, hub.PartCount()); // axle + 2 bearings
        Assert.True(hub.IsAssembly);
    }

    [Fact(DisplayName = "A sub-assembly that contains another sub-assembly rolls up through it")]
    public void WheelRollsUpThroughTheHub()
    {
        var wheel = _catalog.Wheel;

        // 12.00 labor + rim 24.00 + 32 spokes at 0.40 + hub 13.70 + tire 18.00 + tube 4.50
        Assert.Equal(Money.Of(85.00m), wheel.TotalCost());
        // 850 + 160 + 170 + 260 + 95
        Assert.Equal(1535, wheel.TotalWeightGrams());
        // rim + 32 spokes + 3 in the hub + tire + tube
        Assert.Equal(38, wheel.PartCount());
    }

    [Fact(DisplayName = "The finished product rolls up the whole four-level structure")]
    public void BicycleRollsUpEverything()
    {
        var bicycle = _catalog.Bicycle;

        // 40.00 final assembly + frame 168.00 + 2 wheels at 85.00 + saddle 18.00
        Assert.Equal(Money.Of(396.00m), bicycle.TotalCost());
        // frame 2570 + 2 x 1535 + saddle 310
        Assert.Equal(5950, bicycle.TotalWeightGrams());
        // frame 3 + 2 x 38 + saddle 1
        Assert.Equal(80, bicycle.PartCount());
    }

    [Fact(DisplayName = "The frame's own weight and cost are included, not just its parts'")]
    public void AnAssemblyOwnContributionIsCounted()
    {
        var frame = _catalog.Frame;

        Assert.Equal(Money.Of(168.00m), frame.TotalCost()); // 25 welding + 95 + 42 + 6
        Assert.Equal(2570, frame.TotalWeightGrams());       // 30 own + 1800 + 700 + 40
        Assert.Equal(Money.Of(25.00m), frame.AssemblyCost);
    }

    [Fact(DisplayName = "An empty assembly costs only what it costs to build")]
    public void AnEmptyAssemblyIsTheBaseCase()
    {
        var empty = new Assembly("EMPTY", "Nothing In It", Money.Of(7.50m), 12);

        Assert.Equal(Money.Of(7.50m), empty.TotalCost());
        Assert.Equal(12, empty.TotalWeightGrams());
        Assert.Equal(0, empty.PartCount());
        Assert.Empty(empty.Lines);
    }
}
