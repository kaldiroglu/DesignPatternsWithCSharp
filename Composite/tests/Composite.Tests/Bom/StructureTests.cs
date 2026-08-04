using dev.kaldiroglu.Composite.Bom.Domain;
using dev.kaldiroglu.Composite.Bom.Solution;
using Xunit;

namespace dev.kaldiroglu.Composite.Tests.Bom;

/// <summary>
/// The structural rules of the composite: sharing, editing, cycles, and the fact
/// that a single recursive walk covers parts and assemblies alike.
/// </summary>
public class StructureTests
{
    private readonly ProductCatalog.BicycleStructure _catalog = ProductCatalog.CityBicycle();

    [Fact(DisplayName = "Two wheels are one shared object, with the quantity on the line")]
    public void ASubAssemblyIsSharedNotDuplicated()
    {
        var bicycle = _catalog.Bicycle;
        var wheelLine = bicycle.Lines.First(line => line.Component.PartNumber == "WHEEL-ASM");

        Assert.Same(_catalog.Wheel, wheelLine.Component);
        Assert.Equal(2, wheelLine.Quantity);
        // The bicycle has three lines, not four: frame, wheel x2, saddle.
        Assert.Equal(3, bicycle.Lines.Count);
    }

    [Fact(DisplayName = "A line's extended figures multiply the child's roll-up by the quantity")]
    public void ExtendedFiguresMultiplyByQuantity()
    {
        var twoWheels = new BomLine(_catalog.Wheel, 2);

        Assert.Equal(Money.Of(170.00m), twoWheels.ExtendedCost());
        Assert.Equal(3070, twoWheels.ExtendedWeightGrams());
        Assert.Equal(76, twoWheels.ExtendedPartCount());
    }

    [Fact(DisplayName = "A shared sub-assembly knows all of its parents")]
    public void SharedComponentsHaveSeveralParents()
    {
        var tandemFrame = new Assembly("FRAME-TANDEM", "Tandem Frame");
        tandemFrame.Add(_catalog.Wheel, 2);

        // The one wheel object is now used by two different products.
        Assert.Equal(new[] { _catalog.Bicycle, tandemFrame }, _catalog.Wheel.Parents);
    }

    [Fact(DisplayName = "Adding a component that already appears is rejected")]
    public void DuplicateLinesAreRejected()
    {
        var bicycle = _catalog.Bicycle;

        var failure = Assert.Throws<ArgumentException>(() => bicycle.Add(_catalog.Wheel, 1));
        Assert.Contains("already a line", failure.Message);
    }

    [Fact(DisplayName = "A component may not contain itself, directly or indirectly")]
    public void CyclesAreRejected()
    {
        var bicycle = _catalog.Bicycle;
        var wheel = _catalog.Wheel;
        var hub = _catalog.Hub;

        Assert.Throws<ArgumentException>(() => bicycle.Add(bicycle));
        // The bicycle contains the wheel, so the wheel may not contain the bicycle.
        Assert.Throws<ArgumentException>(() => wheel.Add(bicycle));
        // Two levels down is just as illegal.
        Assert.Throws<ArgumentException>(() => hub.Add(bicycle));
    }

    [Fact(DisplayName = "Removing a line detaches the child and clears its parent link")]
    public void RemoveDetachesAChild()
    {
        var bicycle = _catalog.Bicycle;
        var wheel = _catalog.Wheel;

        Assert.True(bicycle.Remove(wheel));
        Assert.Equal(2, bicycle.Lines.Count);
        Assert.Empty(wheel.Parents);
        Assert.False(bicycle.Remove(wheel)); // already gone
    }

    [Fact(DisplayName = "A part exposes no lines, so one recursive walk handles every node")]
    public void ASingleWalkVisitsPartsAndAssembliesAlike()
    {
        Assert.Empty(_catalog.Spoke.Lines);
        // 1 bicycle + 1 frame + 3 frame parts + 1 wheel + 4 wheel parts
        // + 1 hub + 2 hub parts + 1 saddle = 14 distinct nodes
        Assert.Equal(14, CountNodes(_catalog.Bicycle));
        Assert.Equal(1, CountNodes(_catalog.Spoke));
    }

    [Fact(DisplayName = "The tree rendering shows quantities and extended figures")]
    public void TreeRenderingIncludesTheWholeStructure()
    {
        var tree = _catalog.Bicycle.ToTree();

        Assert.Contains("City Bicycle [BIKE-CITY]", tree);
        Assert.Contains("2x 700c Wheel [WHEEL-ASM]", tree);
        Assert.Contains("32x 14g Spoke [SPOKE-14G]", tree);
        Assert.Contains("$170.00", tree); // two wheels, extended
    }

    /// <summary>Counts distinct nodes, ignoring quantities — the shared wheel counts once.</summary>
    private static int CountNodes(BomComponent component)
    {
        var count = 1;
        foreach (var line in component.Lines)
        {
            count += CountNodes(line.Component);
        }

        return count;
    }
}
