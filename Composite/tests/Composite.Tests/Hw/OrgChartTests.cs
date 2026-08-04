using dev.kaldiroglu.Composite.Hw.OrgChart;
using Xunit;

namespace dev.kaldiroglu.Composite.Tests.Hw;

/// <summary>Homework 1: the roll-up, and what a shared report costs it.</summary>
public class OrgChartTests
{
    private readonly IndividualContributor _cem = new("Cem", "designer", 80_000);
    private readonly Manager _engineering;
    private readonly Manager _design;
    private readonly Manager _chief;

    public OrgChartTests()
    {
        _engineering = new Manager("Deniz", "eng manager", 120_000)
            .Add(new IndividualContributor("Ayse", "engineer", 90_000))
            .Add(new IndividualContributor("Bora", "engineer", 85_000));
        _design = new Manager("Ece", "design manager", 115_000).Add(_cem);
        _chief = new Manager("Fatma", "CTO", 180_000).Add(_engineering).Add(_design);
    }

    [Fact(DisplayName = "A leaf answers for itself")]
    public void LeafAnswersForItself()
    {
        Assert.Equal(80_000, _cem.TotalCost());
        Assert.Equal(1, _cem.Headcount());
    }

    [Fact(DisplayName = "A manager counts their own salary as well as their reports'")]
    public void RollUpIncludesTheManager()
    {
        Assert.Equal(295_000, _engineering.TotalCost());   // 120 + 90 + 85
        Assert.Equal(3, _engineering.Headcount());
        Assert.Equal(670_000, _chief.TotalCost());         // 180 + 295 + (115 + 80)
        Assert.Equal(6, _chief.Headcount());
    }

    [Fact(DisplayName = "A shared report is counted twice, and nothing warns")]
    public void SharingSilentlyOverCounts()
    {
        _engineering.Add(_cem);   // the same object, in two places

        Assert.Equal(7, _chief.Headcount());          // Cem counted twice
        Assert.Equal(6, _chief.DistinctHeadcount());  // the true number
        Assert.Equal(750_000, _chief.TotalCost());    // and his salary paid twice
    }

    [Fact(DisplayName = "A cycle is refused, because that one hangs the program")]
    public void CyclesAreRejected()
    {
        Assert.Throws<ArgumentException>(() => _engineering.Add(_chief));
        Assert.Throws<ArgumentException>(() => _chief.Add(_chief));
    }
}
