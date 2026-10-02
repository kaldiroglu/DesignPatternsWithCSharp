using dev.kaldiroglu.Strategy.Pricing.Domain;

namespace dev.kaldiroglu.Strategy.Pricing.Solution;

/// <summary>
/// A <b>ConcreteStrategy</b> that is really a family of them: a flat percentage, for whoever
/// qualifies.
/// <para>
/// The student and staff campaigns are this class twice with different numbers. That is the
/// first thing to notice about a strategy hierarchy — a rule with parameters is one class,
/// not one class per parameter, and the till cannot tell the difference either way.
/// </para>
/// </summary>
public sealed class PercentageOff : IPricingRule
{
    private readonly int _percent;
    private readonly Func<Basket, bool> _qualifies;

    public PercentageOff(string name, int percent, Func<Basket, bool> qualifies)
    {
        Name = name;
        _percent = percent;
        _qualifies = qualifies;
    }

    public static IPricingRule Student() =>
        new PercentageOff("STUDENT", 20, basket => basket.Customer.IsStudent);

    public static IPricingRule Staff() =>
        new PercentageOff("STAFF", 30, basket => basket.Customer.IsStaff);

    public string Name { get; }

    public Money PriceFor(Basket basket) =>
        _qualifies(basket)
            ? basket.ListTotal.PercentOff(_percent)
            : basket.ListTotal;
}
