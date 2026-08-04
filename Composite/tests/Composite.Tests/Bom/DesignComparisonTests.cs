using dev.kaldiroglu.Composite.Bom.Domain;
using dev.kaldiroglu.Composite.Bom.Problem;
using dev.kaldiroglu.Composite.Bom.Solution;
using Xunit;
using ProblemAssembly = dev.kaldiroglu.Composite.Bom.Problem.Assembly;
using ProblemPart = dev.kaldiroglu.Composite.Bom.Problem.Part;

namespace dev.kaldiroglu.Composite.Tests.Bom;

/// <summary>
/// The two designs, side by side — the test that turns the two namespaces into one
/// lesson.
/// </summary>
/// <remarks>
/// <para>
/// The first test is the fair one: both designs are built from the same figures in
/// <see cref="Catalog"/> and reach <b>identical</b> answers. The naive design is not
/// broken and is not a straw man, and no argument for Composite is allowed to rest on
/// pretending otherwise.
/// </para>
/// <para>
/// The rest measure the difference that <em>is</em> real: how much structure each
/// design needs to reach those answers, and what happens when the world changes.
/// </para>
/// </remarks>
public class DesignComparisonTests
{
    private readonly NaiveProductCatalog.BicycleStructure _naive =
        NaiveProductCatalog.CityBicycle();

    private readonly ProductCatalog.BicycleStructure _composite = ProductCatalog.CityBicycle();

    [Fact(DisplayName = "Both designs give exactly the same answers")]
    public void BothDesignsAgree()
    {
        Assert.Equal(NaiveCosting.TotalCost(_naive.Bicycle), _composite.Bicycle.TotalCost());
        Assert.Equal(NaiveShipping.TotalWeightGrams(_naive.Bicycle),
            _composite.Bicycle.TotalWeightGrams());
        Assert.Equal(NaiveCosting.PartCount(_naive.Bicycle), _composite.Bicycle.PartCount());

        // And, for the record, what those answers are.
        Assert.Equal(Money.Of(396.00m), _composite.Bicycle.TotalCost());
        Assert.Equal(5950, _composite.Bicycle.TotalWeightGrams());
        Assert.Equal(80, _composite.Bicycle.PartCount());
    }

    [Fact(DisplayName = "The naive walk visits 85 child entries; the Composite walk visits 13")]
    public void TheCompositeStructureIsFarSmallerToTraverse()
    {
        Assert.Equal(85, NaiveListEntries(_naive.Bicycle));
        Assert.Equal(13, CompositeLines(_composite.Bicycle));
    }

    [Fact(DisplayName = "The naive design needs two wheel objects; the Composite design needs one")]
    public void OnlyTheCompositeDesignCanShare()
    {
        Assert.NotSame(_naive.Wheel1, _naive.Wheel2);

        // In the Composite structure there is one wheel, reached by one line with
        // quantity 2.
        var wheelLine = _composite.Bicycle.Lines
            .First(line => line.Component.PartNumber == "WHEEL-ASM");
        Assert.Same(_composite.Wheel, wheelLine.Component);
        Assert.Equal(Catalog.WheelsPerBicycle, wheelLine.Quantity);
    }

    [Fact(DisplayName = "The same engineering change is right in one design and wrong in the other")]
    public void AnEngineeringChangeDivergesBetweenTheDesigns()
    {
        // "Use 36 spokes per wheel instead of 32."
        _naive.Wheel1.AddPart(new ProblemPart(Catalog.Spoke), 4);
        _composite.Wheel.ChangeQuantity(_composite.Spoke, 36);

        // The Composite design changes both wheels, because there is one wheel.
        Assert.Equal(Money.Of(399.20m), _composite.Bicycle.TotalCost());

        // The naive design changed the wheel the developer happened to reach for, and
        // the product is now quietly wrong by one wheel's worth of spokes.
        Assert.Equal(Money.Of(397.60m), NaiveCosting.TotalCost(_naive.Bicycle));

        // Getting it right in the naive design means remembering the second wheel.
        _naive.Wheel2.AddPart(new ProblemPart(Catalog.Spoke), 4);
        Assert.Equal(Money.Of(399.20m), NaiveCosting.TotalCost(_naive.Bicycle));
    }

    /// <summary>The naive structure: every entry of every child list, recursively.</summary>
    private static int NaiveListEntries(ProblemAssembly assembly)
    {
        var entries = assembly.Parts.Count + assembly.SubAssemblies.Count;
        foreach (var sub in assembly.SubAssemblies)
        {
            entries += NaiveListEntries(sub);
        }

        return entries;
    }

    /// <summary>The Composite structure: every BomLine, recursively, counted once.</summary>
    private static int CompositeLines(BomComponent component)
    {
        var lines = component.Lines.Count;
        foreach (var line in component.Lines)
        {
            lines += CompositeLines(line.Component);
        }

        return lines;
    }
}
