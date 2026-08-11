using System.Globalization;

namespace dev.kaldiroglu.Decorator.Middleware.Domain;

/// <summary>A supplier's price for one item.</summary>
public sealed record Quote(string Sku, decimal Amount, string Currency)
{
    public static Quote Of(string sku, string amount) =>
        new(sku, decimal.Parse(amount, CultureInfo.InvariantCulture), "EUR");

    /// <summary>
    /// Invariant culture on purpose: a Turkish locale would otherwise print 249,00 and the
    /// tests, which assert "249.00", would fail on the machine they were written on.
    /// </summary>
    public override string ToString() =>
        $"{Sku} = {Amount.ToString(CultureInfo.InvariantCulture)} {Currency}";
}
