using dev.kaldiroglu.Composite.Gof.Equipment;
using Xunit;

namespace dev.kaldiroglu.Composite.Tests.Gof;

/// <summary>
/// Unit tests for the equipment example of GoF pp. 170–173.
/// </summary>
/// <remarks>
/// The fixture is the book's own assembly: a cabinet holding a chassis, which
/// holds a bus with one card plus a floppy disk.
/// </remarks>
public class EquipmentCompositeTests
{
    private readonly Cabinet _cabinet = new("PC Cabinet");     // own net  $90.00,  0 W
    private readonly Chassis _chassis = new("PC Chassis");     // own net $210.00, 25 W
    private readonly Bus _bus = new("MCA Bus");                // own net  $75.00, 10 W
    private readonly Card _card = new("16Mbs Token Ring");     //     net $120.00,  8 W
    private readonly FloppyDisk _floppy = new("3.5in Floppy"); //     net  $35.00, 15 W

    public EquipmentCompositeTests()
    {
        _cabinet.Add(_chassis);
        _bus.Add(_card);
        _chassis.Add(_bus);
        _chassis.Add(_floppy);
    }

    [Fact(DisplayName = "A leaf answers from its own state")]
    public void LeafPricesItself()
    {
        Assert.Equal(Currency.Of(120.00m), _card.NetPrice());
        Assert.Equal(8, _card.Power());
    }

    [Fact(DisplayName = "NetPrice() on a composite sums its own price and its subtree")]
    public void NetPriceRollsUpThroughTheTree()
    {
        // bus = 75 + card 120
        Assert.Equal(Currency.Of(195.00m), _bus.NetPrice());
        // chassis = 210 + bus 195 + floppy 35
        Assert.Equal(Currency.Of(440.00m), _chassis.NetPrice());
        // cabinet = 90 + chassis 440
        Assert.Equal(Currency.Of(530.00m), _cabinet.NetPrice());
    }

    [Fact(DisplayName = "Power() rolls up the same way")]
    public void PowerRollsUpThroughTheTree()
    {
        Assert.Equal(18, _bus.Power());     // 10 + 8
        Assert.Equal(58, _chassis.Power()); // 25 + 18 + 15
        Assert.Equal(58, _cabinet.Power()); // 0 + 58 — the cabinet draws nothing itself
    }

    [Fact(DisplayName = "DiscountPrice() applies each node's own rate as it rolls up")]
    public void DiscountPriceRollsUpWithPerNodeRates()
    {
        // card 120 * 0.95 = 114.00; bus own 75 * 0.90 = 67.50
        Assert.Equal(Currency.Of(181.50m), _bus.DiscountPrice());
        // chassis own 210 * 0.85 = 178.50; + bus 181.50; + floppy 35 * 0.90 = 31.50
        Assert.Equal(Currency.Of(391.50m), _chassis.DiscountPrice());
        // cabinet own 90 * 0.80 = 72.00; + chassis 391.50
        Assert.Equal(Currency.Of(463.50m), _cabinet.DiscountPrice());
    }

    [Fact(DisplayName = "Adding equipment changes every ancestor's answer")]
    public void AddingAPartUpdatesTheWholeTree()
    {
        var before = _cabinet.NetPrice();
        _bus.Add(new Card("Ethernet", 6, Currency.Of(60.00m)));

        Assert.Equal(before.Plus(Currency.Of(60.00m)), _cabinet.NetPrice());
        Assert.Equal(64, _cabinet.Power()); // 58 + 6
    }

    [Fact(DisplayName = "Removing equipment does the same in reverse")]
    public void RemovingAPartUpdatesTheWholeTree()
    {
        _chassis.Remove(_floppy);

        Assert.Equal(Currency.Of(405.00m), _chassis.NetPrice()); // 440 - 35
        Assert.Equal(Currency.Of(495.00m), _cabinet.NetPrice()); // 530 - 35
    }

    [Fact(DisplayName = "A simple piece of equipment rejects child operations")]
    public void LeavesRejectChildOperations()
    {
        Assert.Throws<NotSupportedException>(() => _card.Add(_floppy));
        Assert.Throws<NotSupportedException>(() => _floppy.Remove(_card));
        Assert.True(_chassis.IsComposite);
        Assert.True(_cabinet.IsComposite);
    }

    [Fact(DisplayName = "Any Equipment is enumerable, so one walk covers leaves and assemblies")]
    public void TheWholeTreeIsWalkableThroughTheComponentInterface()
    {
        Assert.Equal(5, CountNodes(_cabinet)); // cabinet, chassis, bus, card, floppy
        Assert.Equal(1, CountNodes(_card));    // a leaf is a one-node tree
    }

    private static int CountNodes(Equipment equipment)
    {
        var count = 1;
        foreach (var part in equipment)
        {
            count += CountNodes(part);
        }

        return count;
    }
}
