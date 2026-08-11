namespace dev.kaldiroglu.Decorator.Toast.Problem;

/// <summary>
/// The request that does not fit. A class cannot derive from both CheeseToast and
/// SausageToast, so the price is copied out of each and the shared bread subtracted.
/// </summary>
public class CheeseSausageToast : AbstractToast
{
    public CheeseSausageToast() => NameValue = "Cheese sausage toast";

    public override int CalculatePrice()
    {
        // Copied out of CheeseToast (5) and SausageToast (6), less the bread that would
        // otherwise be charged for twice. The subtraction is a guess, because no class in
        // this namespace ever says what the bread costs on its own.
        return 5 + 6 - 3;
    }
}
