using dev.kaldiroglu.Composite.Bom.Domain;
using dev.kaldiroglu.Composite.Bom.Solution;
using Xunit;

namespace dev.kaldiroglu.Composite.Tests.Bom;

/// <summary>
/// GoF's "Caching to improve performance" (p. 169) and the obligation it creates:
/// a change anywhere below a node must invalidate that node's cached answer.
/// </summary>
public class CacheInvalidationTests
{
    private readonly ProductCatalog.BicycleStructure _catalog = ProductCatalog.CityBicycle();

    [Fact(DisplayName = "A roll-up is computed once and then remembered")]
    public void RollUpsAreMemoized()
    {
        var bicycle = _catalog.Bicycle;
        Assert.False(bicycle.IsCostCached);

        var first = bicycle.TotalCost();
        Assert.True(bicycle.IsCostCached);

        // The same value comes back — no second walk of the tree.
        Assert.Equal(first, bicycle.TotalCost());
    }

    [Fact(DisplayName = "A change two levels down invalidates the root's cache")]
    public void ADeepChangeInvalidatesEveryAncestor()
    {
        var bicycle = _catalog.Bicycle;
        var wheel = _catalog.Wheel;
        var hub = _catalog.Hub;

        bicycle.TotalCost(); // warm every cache on the path
        wheel.TotalCost();
        hub.TotalCost();
        Assert.True(bicycle.IsCostCached);

        // Add a dust cap to the hub — the deepest assembly in the structure.
        hub.Add(new Part("CAP-DUST", "Dust Cap", Money.Of(0.75m), 4), 2);

        Assert.False(hub.IsCostCached);
        Assert.False(wheel.IsCostCached);
        Assert.False(bicycle.IsCostCached); // the invalidation travelled upwards
    }

    [Fact(DisplayName = "An engineering change on a shared sub-assembly is felt everywhere")]
    public void ChangingAQuantityUpdatesEveryRollUpAbove()
    {
        var bicycle = _catalog.Bicycle;
        var wheel = _catalog.Wheel;

        Assert.Equal(Money.Of(396.00m), bicycle.TotalCost());
        Assert.Equal(5950, bicycle.TotalWeightGrams());

        // 32 -> 36 spokes per wheel: +4 spokes at $0.40 and 5 g each.
        wheel.ChangeQuantity(_catalog.Spoke, 36);

        Assert.Equal(Money.Of(86.60m), wheel.TotalCost()); // 85.00 + 1.60
        Assert.Equal(1555, wheel.TotalWeightGrams());      // 1535 + 20
        // Both wheels changed, so the product gains 2 x 1.60 and 2 x 20 g.
        Assert.Equal(Money.Of(399.20m), bicycle.TotalCost());
        Assert.Equal(5990, bicycle.TotalWeightGrams());
        Assert.Equal(88, bicycle.PartCount());             // 80 + 2 x 4
    }

    [Fact(DisplayName = "Removing a component invalidates the ancestors too")]
    public void RemovingAComponentInvalidatesAncestors()
    {
        var bicycle = _catalog.Bicycle;
        var wheel = _catalog.Wheel;

        bicycle.TotalCost();
        wheel.Remove(_catalog.Hub);

        Assert.False(bicycle.IsCostCached);
        Assert.Equal(Money.Of(71.30m), wheel.TotalCost());    // 85.00 - 13.70
        Assert.Equal(Money.Of(368.60m), bicycle.TotalCost()); // 396.00 - 2 x 13.70
    }

    [Fact(DisplayName = "Attaching a shared sub-assembly to a new parent keeps both parents correct")]
    public void InvalidationFollowsEveryParent()
    {
        var bicycle = _catalog.Bicycle;
        var wheel = _catalog.Wheel;

        var spareWheelset = new Assembly("WHEELSET-SPARE", "Spare Wheelset");
        spareWheelset.Add(wheel, 2);

        bicycle.TotalCost();
        spareWheelset.TotalCost();
        Assert.True(bicycle.IsCostCached);
        Assert.True(spareWheelset.IsCostCached);

        wheel.ChangeQuantity(_catalog.Spoke, 36);

        // One change, two independent products to correct.
        Assert.False(bicycle.IsCostCached);
        Assert.False(spareWheelset.IsCostCached);
        Assert.Equal(Money.Of(173.20m), spareWheelset.TotalCost());
    }
}
