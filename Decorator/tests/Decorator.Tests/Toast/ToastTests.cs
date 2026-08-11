using dev.kaldiroglu.Decorator.Toast.Problem;
using dev.kaldiroglu.Decorator.Toast.Solution;
using Xunit;

namespace dev.kaldiroglu.Decorator.Tests.Toast;

/// <summary>
/// The Ayvalik toast shop, in both designs. Every figure is the one the Java port asserts.
/// </summary>
public class ToastTests
{
    private static IToastable Full() =>
        new Salad(new Ketchup(new Tomato(new Sausage(new Cheese(new ToastBread())))));

    [Fact(DisplayName = "the naive menu prices each combination as a welded number")]
    public void TheNaiveMenu()
    {
        Assert.Equal(5, new CheeseToast().CalculatePrice());
        Assert.Equal(6, new SausageToast().CalculatePrice());
        Assert.Equal(8, new CheeseSausageToast().CalculatePrice()); // 5 + 6 - 3, a guess
    }

    [Fact(DisplayName = "a class per combination: 2^5 - 1 = 31 for five toppings")]
    public void TheCombinatorialCount()
    {
        Assert.Equal(31, (1 << 5) - 1);
        Assert.Equal(31, 5 + 10 + 10 + 5 + 1); // singles, pairs, triples, quads, the lot
    }

    [Fact(DisplayName = "five topping classes price every combination")]
    public void TheDecoratedToast()
    {
        Assert.Equal(5, new ToastBread().CalculatePrice());
        Assert.Equal(16, Full().CalculatePrice());
        Assert.Equal(5, Full().GetToppings().Count);
    }

    [Fact(DisplayName = "the same topping twice — one line, and no class for it anywhere")]
    public void TheSameToppingTwice() =>
        Assert.Equal(11, new Cheese(new Cheese(new ToastBread())).CalculatePrice());

    [Fact(DisplayName = "a decorator that multiplies: where it sits changes the bill")]
    public void PromotionPosition()
    {
        var discountOutside = new Promotion(Full(), "student discount", 25);
        var discountInside = new Salad(new Ketchup(new Tomato(new Sausage(
            new Cheese(new Promotion(new ToastBread(), "student discount", 25))))));

        Assert.Equal(12, discountOutside.CalculatePrice()); // 25% off everything
        Assert.Equal(14, discountInside.CalculatePrice());  // 25% off the bread only
        Assert.NotEqual(discountOutside.CalculatePrice(), discountInside.CalculatePrice());
    }

    [Fact(DisplayName = "a promotion is not a topping, so it adds none")]
    public void APromotionIsNotATopping() =>
        Assert.Equal(5, new Promotion(Full(), "student discount", 25).GetToppings().Count);

    [Fact(DisplayName = "a topping must be added to something")]
    public void NullComponentIsRejected() =>
        Assert.Throws<ArgumentNullException>(() => new Cheese(null!));
}
