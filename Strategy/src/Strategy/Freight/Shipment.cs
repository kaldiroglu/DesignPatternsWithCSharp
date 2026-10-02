namespace dev.kaldiroglu.Strategy.Freight;

/// <summary>
/// One parcel, described the way every carrier's rate card needs it described.
/// <para>
/// The awkward part of freight is that no two carriers rate on the same thing. One charges
/// by weight, one by the volume the parcel occupies on the van, one by which zone it is
/// going to. So the shipment carries all of it, and each rating algorithm reads the part it
/// cares about.
/// </para>
/// </summary>
/// <param name="FromZone">where it is collected</param>
/// <param name="ToZone">where it is going</param>
/// <param name="Grams">actual weight</param>
/// <param name="LengthCm">longest side</param>
/// <param name="WidthCm">second side</param>
/// <param name="HeightCm">third side</param>
public sealed record Shipment(string FromZone, string ToZone,
                              int Grams, int LengthCm, int WidthCm, int HeightCm)
{
    public int Grams { get; } = Grams >= 1
        ? Grams
        : throw new ArgumentException("a shipment has to weigh something", nameof(Grams));

    /// <summary>Volume in cubic centimeters.</summary>
    public int VolumeCm3 => LengthCm * WidthCm * HeightCm;

    /// <summary>
    /// Volumetric weight, in grams, on the divisor Turkish carriers call <i>desi</i>.
    /// <para>
    /// One desi is 3000 cubic centimeters, and it counts as one kilogram. A large light
    /// parcel is charged as though it were heavy, because what it costs the carrier is the
    /// space on the van rather than the load on the axle.
    /// </para>
    /// </summary>
    public int DesiGrams => VolumeCm3 * 1000 / 3000;

    /// <summary>What a carrier that charges for whichever is greater will use.</summary>
    public int ChargeableGrams => Math.Max(Grams, DesiGrams);

    public bool IsDomestic => FromZone == ToZone;
}
