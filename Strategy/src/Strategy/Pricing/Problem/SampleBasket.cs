using dev.kaldiroglu.Strategy.Pricing.Domain;

namespace dev.kaldiroglu.Strategy.Pricing.Problem;

/// <summary>Ceyda's basket: three books at 400 lira, and she is a student. Every stage prices it.</summary>
internal static class SampleBasket
{
    internal static Basket Build() =>
        Basket.Of(Customer.Student("Ceyda"), new Line("java-book", "book", Money.Of("400.00"), 3));
}
