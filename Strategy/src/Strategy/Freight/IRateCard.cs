namespace dev.kaldiroglu.Strategy.Freight;

/// <summary>
/// The <b>Strategy</b>: one carrier's way of pricing a shipment.
/// <para>
/// Two members, and both are about the algorithm. A rate card is handed a shipment and
/// answers what it charges. It does not know which other carriers exist, does not decide
/// whether it should be the carrier used, and does not know that a quote is being compared
/// with anything.
/// </para>
/// <para>
/// The interface is deliberately not <em>price per kilo</em>. Three of the four cards below
/// could be expressed that way and the fourth could not, which is the same lesson the
/// pricing example teaches with buy-two-get-one: the shape of the interface is decided when
/// you know least about the family.
/// </para>
/// </summary>
public interface IRateCard
{
    /// <summary>The carrier's name, for the quote and the audit trail.</summary>
    string Carrier { get; }

    /// <summary>What this carrier charges to move the shipment.</summary>
    Money Quote(Shipment shipment);
}
