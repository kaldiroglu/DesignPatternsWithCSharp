namespace dev.kaldiroglu.Decorator.Hw.FeeEngine;

/// <summary>
/// The <b>Decorator</b>: something that changes an amount. Rounding to money happens once,
/// here, so every adjustment rounds the same way.
/// </summary>
public abstract class Adjustment : ICharge
{
    protected readonly ICharge Component;

    protected Adjustment(ICharge component) =>
        Component = component ?? throw new ArgumentNullException(nameof(component));

    protected static decimal Money(decimal value) =>
        decimal.Round(value, 2, MidpointRounding.AwayFromZero);

    protected abstract decimal Adjust(decimal @base);

    public decimal Amount() => Money(Adjust(Component.Amount()));
}
