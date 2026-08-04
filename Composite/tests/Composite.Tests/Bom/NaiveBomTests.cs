using dev.kaldiroglu.Composite.Bom.Domain;
using dev.kaldiroglu.Composite.Bom.Problem;
using Xunit;

namespace dev.kaldiroglu.Composite.Tests.Bom;

/// <summary>
/// The naive design, tested twice over: first that it <b>works</b>, then that working
/// is not the same as being a good design.
/// </summary>
/// <remarks>
/// The first two tests are the fair part — the naive code computes exactly the figures
/// the Composite version computes, so nobody can dismiss it as a straw man. Everything
/// after that measures what it costs to keep those answers right.
/// </remarks>
public class NaiveBomTests
{
    private readonly NaiveProductCatalog.BicycleStructure _catalog =
        NaiveProductCatalog.CityBicycle();

    // --- It works ----------------------------------------------------------

    [Fact(DisplayName = "The naive design computes the correct totals")]
    public void TheNaiveDesignIsCorrect()
    {
        var bicycle = _catalog.Bicycle;

        Assert.Equal(Money.Of(396.00m), NaiveCosting.TotalCost(bicycle));
        Assert.Equal(5950, NaiveShipping.TotalWeightGrams(bicycle));
        Assert.Equal(80, NaiveCosting.PartCount(bicycle));
    }

    [Fact(DisplayName = "It is correct at every level, not only at the root")]
    public void TheNaiveDesignIsCorrectAtEveryLevel()
    {
        Assert.Equal(Money.Of(13.70m), NaiveCosting.TotalCost(_catalog.Hub1));
        Assert.Equal(Money.Of(85.00m), NaiveCosting.TotalCost(_catalog.Wheel1));
        Assert.Equal(Money.Of(168.00m), NaiveCosting.TotalCost(_catalog.Frame));
        Assert.Equal(1535, NaiveShipping.TotalWeightGrams(_catalog.Wheel1));
    }

    // --- What it costs -----------------------------------------------------

    [Fact(DisplayName = "Cost #1: an operation cannot be a method, so its parameter is object")]
    public void OperationsBecomeStaticFunctionsOverObject()
    {
        // There is no type to declare, so the client's own variable must be object
        // and the compiler can no longer tell it anything useful.
        object anything = _catalog.Wheel1;
        Assert.Equal(Money.Of(85.00m), NaiveCosting.TotalCost(anything));

        // And a plain string type-checks perfectly well at the call site.
        Assert.Throws<ArgumentException>(() => NaiveCosting.TotalCost("a bicycle, surely?"));
    }

    [Fact(DisplayName = "Cost #2: the same recursion exists three times")]
    public void TheSameWalkIsWrittenThreeTimes()
    {
        var bicycle = _catalog.Bicycle;

        // Three separate implementations, each with its own type chain and its own
        // pair of loops. They agree today only because they were written carefully;
        // nothing enforces it.
        Assert.Equal(Money.Of(396.00m), NaiveCosting.TotalCost(bicycle)); // walk 1
        Assert.Equal(80, NaiveCosting.PartCount(bicycle));                // walk 2
        Assert.Equal(5950, NaiveShipping.TotalWeightGrams(bicycle));      // walk 3
    }

    [Fact(DisplayName = "Cost #3: no quantities, so every query walks 85 list entries")]
    public void QuantitiesBecomeDuplicatedListEntries()
    {
        // 32 spokes are 32 entries, and the wheel's 39 entries are counted twice
        // because the wheel itself exists twice.
        Assert.Equal(85, ListEntries(_catalog.Bicycle));
        // One wheel's parts list: rim + 32 spokes + tire + tube.
        Assert.Equal(Catalog.SpokesPerWheel + 3, _catalog.Wheel1.Parts.Count);

        // The Composite version reaches the same totals from 13 line entries.
        // See DesignComparisonTests.
    }

    [Fact(DisplayName = "Cost #4: the two wheels are different objects and can drift apart")]
    public void TheTwoWheelsAreDifferentObjectsAndCanDriftApart()
    {
        Assert.NotSame(_catalog.Wheel1, _catalog.Wheel2);
        Assert.Equal(NaiveCosting.TotalCost(_catalog.Wheel1),
            NaiveCosting.TotalCost(_catalog.Wheel2));

        // An engineering change applied the way a developer naturally would — to
        // "the wheel", of which there are secretly two.
        _catalog.Wheel1.AddPart(new Part(Catalog.Spoke), 4); // 32 -> 36 spokes

        Assert.Equal(Money.Of(86.60m), NaiveCosting.TotalCost(_catalog.Wheel1));
        Assert.Equal(Money.Of(85.00m), NaiveCosting.TotalCost(_catalog.Wheel2)); // untouched!

        // The product is now wrong: it should be $399.20 with both wheels changed.
        Assert.Equal(Money.Of(397.60m), NaiveCosting.TotalCost(_catalog.Bicycle));
    }

    [Fact(DisplayName = "Cost #5: a new kind of item breaks every existing client")]
    public void ANewKindOfItemBreaksEveryExistingClient()
    {
        var coating = new Service(Catalog.PowderCoating);

        var costFailure = Assert.Throws<ArgumentException>(() => NaiveCosting.TotalCost(coating));
        Assert.Contains("Service", costFailure.Message);

        Assert.Throws<ArgumentException>(() => NaiveCosting.PartCount(coating));
        Assert.Throws<ArgumentException>(() => NaiveShipping.TotalWeightGrams(coating));

        // Three clients to edit — and Assembly needs a third collection before a
        // Service can even be attached to one. Compare
        // Solution.ExtensibilityTests, where the same requirement costs one class
        // and no edits at all.
    }

    /// <summary>Counts the child-list entries a full traversal has to walk.</summary>
    private static int ListEntries(Assembly assembly)
    {
        var entries = assembly.Parts.Count + assembly.SubAssemblies.Count;
        foreach (var sub in assembly.SubAssemblies)
        {
            entries += ListEntries(sub);
        }

        return entries;
    }
}
