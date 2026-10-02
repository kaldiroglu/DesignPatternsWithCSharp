namespace dev.kaldiroglu.Strategy.Pricing.Domain;

/// <summary>
/// What the customer is handed, and the promise the whole example turns on.
/// <para>
/// The receipt names the campaign that was applied and states what it saved. Both require
/// the same basket to be priced <em>twice</em> — once at shelf price and once under the
/// campaign — which is the operation the third naive design cannot perform at any price.
/// </para>
/// </summary>
/// <param name="Campaign">what to print above the total</param>
/// <param name="List">the shelf price of the basket</param>
/// <param name="Paid">what the customer actually pays</param>
public sealed record Receipt(string Campaign, Money List, Money Paid)
{
    /// <summary>What the campaign took off. Zero when no campaign applied.</summary>
    public Money Saved => List.Minus(Paid);

    public override string ToString() =>
        $"{Campaign,-22} list {List,8}   paid {Paid,8}   saved {Saved,8}";
}
