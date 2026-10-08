namespace dev.kaldiroglu.Observer.Price.Problem;

/// <summary>
/// A reader of the price: fires once when the price reaches its limit.
/// <para>
/// The promise of the story: an alert at 105 fires whenever the price reaches 105.
/// </para>
/// </summary>
public sealed class PriceAlert(int limit)
{
    private bool fired;

    public void Check(int price)
    {
        if (price >= limit)
        {
            fired = true;
        }
    }

    public bool Fired => fired;
}
