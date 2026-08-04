using dev.kaldiroglu.Composite.Bom.Domain;
using dev.kaldiroglu.Composite.Bom.Solution;
using Xunit;

namespace dev.kaldiroglu.Composite.Tests.Bom;

/// <summary>
/// GoF's third consequence (p. 166), as a runnable claim: <em>"Newly defined
/// Composite or Leaf subclasses work automatically with existing structures and
/// existing client code."</em>
/// </summary>
/// <remarks>
/// <para>
/// <c>Service</c> is a new kind of Leaf — a subcontracted operation that costs money,
/// weighs nothing, and is not a part. Adding it required writing that one class.
/// Nothing else in the namespace changed, and none of the operations exercised below
/// were told it exists.
/// </para>
/// <para>
/// The mirror image is <c>Problem.NaiveBomTests.ANewKindOfItemBreaksEveryExistingClient</c>,
/// where the same requirement breaks three clients and needs a third collection on the
/// assembly.
/// </para>
/// </remarks>
public class ExtensibilityTests
{
    private readonly ProductCatalog.BicycleStructure _catalog = ProductCatalog.CityBicycle();
    private readonly Service _coating = new(Catalog.PowderCoating); // $14.00, 0 g, 0 parts

    [Fact(DisplayName = "A Service answers the Component questions in its own way")]
    public void AServiceIsALeafWithDifferentAnswers()
    {
        Assert.Equal(Money.Of(14.00m), _coating.TotalCost());
        Assert.Equal(0, _coating.TotalWeightGrams()); // an operation adds no mass
        Assert.Equal(0, _coating.PartCount());        // and is not a purchasable part
        Assert.False(_coating.IsAssembly);
    }

    [Fact(DisplayName = "An existing Assembly accepts it with no change to Assembly")]
    public void AnExistingCompositeAcceptsTheNewLeaf()
    {
        var frame = _catalog.Frame;
        frame.Add(_coating);

        Assert.Equal(Money.Of(182.00m), frame.TotalCost()); // 168.00 + 14.00
        Assert.Equal(2570, frame.TotalWeightGrams());       // unchanged
        Assert.Equal(3, frame.PartCount());                 // unchanged
    }

    [Fact(DisplayName = "The roll-up above it is correct without anyone editing the roll-up")]
    public void TheRollUpAbsorbsItAutomatically()
    {
        var bicycle = _catalog.Bicycle;
        bicycle.TotalCost(); // warm the caches first, to prove invalidation still works

        _catalog.Frame.Add(_coating);

        Assert.Equal(Money.Of(410.00m), bicycle.TotalCost()); // 396.00 + 14.00
        Assert.Equal(5950, bicycle.TotalWeightGrams());       // unchanged
        Assert.Equal(80, bicycle.PartCount());                // unchanged
    }

    [Fact(DisplayName = "Clients written before the class existed handle it correctly")]
    public void PreExistingClientsKeepWorking()
    {
        _catalog.Frame.Add(_coating);

        // The tree printer: never edited, renders the new leaf.
        var tree = _catalog.Bicycle.ToTree();
        Assert.Contains("Powder Coating [SVC-COAT]", tree);

        // A shipping estimator written against BomComponent: still right, because a
        // service contributes no mass.
        Assert.Equal(Money.Of(29.40m), ShippingEstimate(_catalog.Bicycle));

        // And a generic recursive walk still terminates, because a Service is a leaf
        // and reports no lines.
        Assert.Equal(15, CountNodes(_catalog.Bicycle)); // 14 before, plus the service
    }

    [Fact(DisplayName = "It nests like any other component")]
    public void AServiceCanAppearAnywhereInTheStructure()
    {
        _catalog.Hub.Add(_coating); // four levels down

        Assert.Equal(Money.Of(27.70m), _catalog.Hub.TotalCost());   // 13.70 + 14.00
        Assert.Equal(Money.Of(99.00m), _catalog.Wheel.TotalCost()); // 85.00 + 14.00
        // Two wheels, so the product picks it up twice.
        Assert.Equal(Money.Of(424.00m), _catalog.Bicycle.TotalCost()); // 396.00 + 28.00
    }

    /// <summary>The same estimator the demo uses — written against the Component only.</summary>
    private static Money ShippingEstimate(BomComponent component)
    {
        var kilos = Math.Max(1, (component.TotalWeightGrams() + 999) / 1000);
        return Money.Of(4.90m).Times(kilos);
    }

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
