using dev.kaldiroglu.Iterator.Hw.Bom.Composite;

namespace dev.kaldiroglu.Iterator.Hw.Bom;

/// <summary>
/// One part, and how many of it the whole product needs.
/// </summary>
/// <remarks>
/// The quantity is multiplied down the tree: two wheels, each with thirty-six spokes, gives
/// one line of seventy-two spokes from the bicycle's point of view.
/// </remarks>
public sealed record PartLine(Part Part, int Quantity)
{
    public override string ToString() => $"{Quantity} x {Part.Name}";
}
