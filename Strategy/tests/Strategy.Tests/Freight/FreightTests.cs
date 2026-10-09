using System.Reflection;
using dev.kaldiroglu.Strategy.Freight;
using Xunit;

namespace dev.kaldiroglu.Strategy.Tests.Freight;

/// <summary>
/// Four carriers, four ways of arriving at a price, and one shipment they all disagree
/// about. Ported from the Java <c>freight.FreightTest</c>.
/// </summary>
public class FreightTests
{
    /// <summary>A large light parcel: light on the scale, expensive on the van.</summary>
    private static readonly Shipment Pillow = new("TR", "TR", 900, 50, 40, 30);

    /// <summary>A small heavy one: the opposite trade.</summary>
    private static readonly Shipment Books = new("TR", "TR", 4200, 25, 20, 10);

    private static IRateCard Desi() => new ByDesi("Yurtici", Money.Of("38.00"), 1);

    private static IRateCard Bands() =>
        new ByWeightBand("Aras",
        [
            new ByWeightBand.Band(1000, Money.Of("45.00")),
            new ByWeightBand.Band(5000, Money.Of("70.00")),
            new ByWeightBand.Band(10000, Money.Of("110.00"))
        ],
        Money.Of("190.00"));

    private static ByZone Zones() =>
        new("UPS",
            new Dictionary<string, Money> { ["TR"] = Money.Of("60.00"), ["DE"] = Money.Of("240.00") },
            Money.Of("12.00"), 15);

    private static IRateCard Flat() => new FlatRate("Marketplace", Money.Of("89.90"));

    private static CarrierBoard Board() => new(Desi(), Bands(), Zones(), Flat());

    [Fact(DisplayName = "volume, not weight: a pillow is charged as twenty kilos")]
    public void DesiIsAboutSpace()
    {
        // 50 x 40 x 30 is 60,000 cm3, which is 20 desi: 20 kg chargeable against 0.9 kg actual.
        Assert.Equal(60_000, Pillow.VolumeCm3);
        Assert.Equal(20_000, Pillow.DesiGrams);
        Assert.Equal(20_000, Pillow.ChargeableGrams);
        Assert.Equal(Money.Of("760.00"), Desi().Quote(Pillow));   // 20 x 38.00
    }

    [Fact(DisplayName = "the same card charges the heavy parcel by its weight instead")]
    public void DesiFallsBackToWeight()
    {
        // 25 x 20 x 10 is 5,000 cm3, under two desi, so the 4.2 kg on the scale wins.
        Assert.Equal(1_666, Books.DesiGrams);
        Assert.Equal(4_200, Books.ChargeableGrams);
        Assert.Equal(Money.Of("190.00"), Desi().Quote(Books));    // 5 x 38.00, rounded up
    }

    [Fact(DisplayName = "a band table steps: one gram over an edge costs a whole band more")]
    public void BandsAreALookupNotARate()
    {
        Assert.Equal(Money.Of("70.00"), Bands().Quote(Books));    // 4.2 kg, the 5 kg band

        var justOver = new Shipment("TR", "TR", 5_001, 10, 10, 10);
        Assert.Equal(Money.Of("70.00"), Bands().Quote(new Shipment("TR", "TR", 5_000, 10, 10, 10)));
        Assert.Equal(Money.Of("110.00"), Bands().Quote(justOver));

        // A rate per kilo cannot express that, which is why the interface is a method.
        Assert.NotEqual(Bands().Quote(justOver), Bands().Quote(Books));
    }

    [Fact(DisplayName = "a zone card prices the route first and the parcel second")]
    public void ZonesAddASurcharge()
    {
        // 60.00 base + 5 kg x 12.00 = 120.00, then 15% fuel = 138.00
        Assert.Equal(Money.Of("138.00"), Zones().Quote(Books));
    }

    [Fact(DisplayName = "the fuel feed moves one card, and nothing else")]
    public void TheSurchargeIsOneCardsBusiness()
    {
        var ups = Zones();
        var before = ups.Quote(Books);

        ups.SetFuelSurchargePercent(30);

        Assert.Equal(Money.Of("138.00"), before);
        Assert.Equal(Money.Of("156.00"), ups.Quote(Books));       // 120.00 + 30%
        Assert.Equal(Money.Of("190.00"), Desi().Quote(Books));    // untouched
    }

    [Fact(DisplayName = "a card may ignore the shipment entirely and still be a card")]
    public void FlatRateIgnoresEverything()
    {
        Assert.Equal(Flat().Quote(Pillow), Flat().Quote(Books));
        Assert.Equal(Money.Of("89.90"), Flat().Quote(Pillow));
    }

    [Fact(DisplayName = "one desk, four carriers, and the cheapest depends on the parcel")]
    public void TheCheapestIsNotAlwaysTheSameCarrier()
    {
        var forPillow = Board().QuoteAll(Pillow);
        var forBooks = Board().QuoteAll(Books);

        Assert.Equal(4, forPillow.Count);
        Assert.Equal(["Yurtici", "Aras", "UPS", "Marketplace"],
            forPillow.Select(q => q.Carrier).ToList());

        // Both parcels priced by all four.
        Assert.Equal([Money.Of("760.00"), Money.Of("190.00"), Money.Of("345.00"), Money.Of("89.90")],
            forPillow.Select(q => q.Price).ToList());
        Assert.Equal([Money.Of("190.00"), Money.Of("70.00"), Money.Of("138.00"), Money.Of("89.90")],
            forBooks.Select(q => q.Price).ToList());

        // The flat rate wins the big light parcel; the band table wins the small heavy one.
        Assert.Equal("Marketplace", Board().CheapestFor(Pillow).Carrier);
        Assert.Equal("Aras", Board().CheapestFor(Books).Carrier);
        Assert.NotEqual(Board().CheapestFor(Pillow).Carrier, Board().CheapestFor(Books).Carrier);
    }

    /// <summary>
    /// A fifth carrier. Java writes it as an anonymous class inside the test; C# has no
    /// anonymous classes that implement an interface, so it is a nested class here.
    /// </summary>
    private sealed class Bike : IRateCard
    {
        public string Carrier => "Kurye";

        public Money Quote(Shipment shipment) =>
            shipment.IsDomestic && shipment.ChargeableGrams <= 5_000
                ? Money.Of("35.00")
                : Money.Of("999.00");
    }

    [Fact(DisplayName = "a fifth carrier is one class, and the board is the only edit")]
    public void AddingACarrier()
    {
        var board = Board().Add(new Bike());

        Assert.Equal(5, board.Size);
        Assert.Equal("Kurye", board.CheapestFor(Books).Carrier);
        Assert.Equal(Money.Of("35.00"), board.CheapestFor(Books).Price);
    }

    [Fact(DisplayName = "a carrier that does not serve the destination says so")]
    public void UnservedZonesAreRejected()
    {
        var toJapan = new Shipment("TR", "JP", 2_000, 20, 20, 20);
        Assert.Throws<ArgumentException>(() => Zones().Quote(toJapan));
    }

    [Fact(DisplayName = "the context holds a card and never asks what kind it is")]
    public void TheDeskOnlyForwards()
    {
        var desk = new ShippingDesk(Desi());
        Assert.Equal("Yurtici", desk.Carrier);

        desk.SetCard(Flat());                                     // the same desk

        Assert.Equal("Marketplace", desk.Carrier);
        Assert.Equal(Money.Of("89.90"), desk.Book(Pillow).Price);
        Assert.All(
            typeof(ShippingDesk).GetFields(BindingFlags.Instance | BindingFlags.Static
                                           | BindingFlags.Public | BindingFlags.NonPublic
                                           | BindingFlags.DeclaredOnly),
            f => Assert.Equal(typeof(IRateCard), f.FieldType));
    }
}
