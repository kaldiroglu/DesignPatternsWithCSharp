namespace dev.kaldiroglu.Decorator.Toast.Problem;

/// <summary>One item on the menu, and one class for it.</summary>
public class CheeseToast : AbstractToast
{
    public CheeseToast() => NameValue = "Cheese toast";

    public override int CalculatePrice()
    {
        // 5 = bread plus cheese, welded into one number. Neither part exists on its own
        // anywhere here, so neither can be repriced on its own.
        return 5;
    }
}
