namespace dev.kaldiroglu.Decorator.Hw.FeeEngine;

using System.Globalization;

/// <summary>The <b>ConcreteComponent</b>: what the customer put in the basket.</summary>
public sealed class BasketTotal : ICharge
{
    private readonly decimal _amount;

    public BasketTotal(string amount) =>
        _amount = decimal.Round(decimal.Parse(amount, CultureInfo.InvariantCulture), 2,
            MidpointRounding.AwayFromZero);

    public decimal Amount() => _amount;
}
