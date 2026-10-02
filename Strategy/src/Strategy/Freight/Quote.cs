namespace dev.kaldiroglu.Strategy.Freight;

/// <summary>
/// What one carrier would charge for one shipment.
/// </summary>
/// <param name="Carrier">who quoted</param>
/// <param name="Price">what they want for it</param>
public sealed record Quote(string Carrier, Money Price)
{
    public override string ToString() => $"{Carrier,-10} {Price,8}";
}
