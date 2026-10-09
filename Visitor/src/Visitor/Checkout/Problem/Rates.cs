namespace dev.kaldiroglu.Visitor.Checkout.Problem;

/// <summary>The shop's own tax rates, in percent, and shipping costs.</summary>
public static class Rates
{
    public const int BookTax = 5;
    public const int FoodTax = 1;
    public const int ElectronicsTax = 20;
    public const int StandardTax = 20;

    public const int BookShipping = 10;
    public const int FoodShipping = 15;
    public const int ElectronicsShipping = 25;
    public const int StandardShipping = 10;
}
